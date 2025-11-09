using AZ.Integrator.Procurement.Application.Common.BackgroundJobs;
using AZ.Integrator.Procurement.Application.Common.Email;
using AZ.Integrator.Procurement.Application.Common.Email.Models;
using AZ.Integrator.Procurement.Application.Common.Settings;
using AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.JobCommands.MarkOrderAsSent;
using AZ.Integrator.Procurement.Contracts.PartDefinitionOrders;
using AZ.Integrator.Procurement.Contracts.Suppliers;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.JobCommands.SendOrderEmailToSupplier;

public class SendOrderEmailToSupplierJobCommandHandler(
    IProcurementDataViewContext dataViewContext,
    IEmailService emailService,
    IEmailTemplateRenderer templateRenderer,
    IOptions<ApplicationSettings> settings,
    IBackgroundJobClient backgroundJobClient)
    : IRequestHandler<SendOrderEmailToSupplierJobCommand>
{
    private PerformContext _ctx = null!;
    
    public async ValueTask<Unit> Handle(SendOrderEmailToSupplierJobCommand command, CancellationToken cancellationToken)
    {
        _ctx = command.PerformContext;

        cancellationToken.ThrowIfCancellationRequested();

        var order = await LoadOrderAsync(command, cancellationToken);
        if (order is null) return Unit.Value;

        var supplier = await LoadSupplierAsync(order.SupplierId, command.TenantId, cancellationToken);
        if (supplier is null) return Unit.Value;

        var emailContent = await BuildEmailContentAsync(order, supplier, cancellationToken);
        if (emailContent is null) return Unit.Value;

        await SendEmailsToSupplierAsync(order, supplier, emailContent, cancellationToken);
        
        ScheduleMarkOrderAsSentJob(command);

        return Unit.Value;
    }

    private void ScheduleMarkOrderAsSentJob(SendOrderEmailToSupplierJobCommand command)
    {
        _ctx.Step("Scheduling job to mark order as sent...");
        
        backgroundJobClient.Enqueue<MarkOrderAsSentJob>(
            job => job.Execute(new MarkOrderAsSentJobCommand
            {
                OrderNumber = command.OrderNumber,
                TenantId = command.TenantId
            }, null));

        _ctx.Success("Job scheduled successfully");
    }

    private async Task<PartDefinitionsOrderViewModel?> LoadOrderAsync(
        SendOrderEmailToSupplierJobCommand command,
        CancellationToken cancellationToken)
    {
        _ctx.Step($"Starting loading order '{command.OrderNumber}' for tenant {command.TenantId}...");

        var order = await dataViewContext.Orders
            .AsNoTracking()
            .Include(x => x.FurnitureLines)
                .ThenInclude(x => x.PartLines)
            .Where(x => x.Number == command.OrderNumber)
            .Where(x => x.TenantId == command.TenantId)
            .SingleOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            _ctx.Error($"Order '{command.OrderNumber}' not found for tenant {command.TenantId}");
            return null;
        }

        _ctx.Success("Order loaded successfully");
        return order;
    }

    private async Task<SupplierViewModel?> LoadSupplierAsync(
        int supplierId,
        Guid tenantId,
        CancellationToken cancellationToken)
    {
        _ctx.Step($"Starting loading supplier ID '{supplierId}' for tenant {tenantId}...");

        var supplier = await dataViewContext.Suppliers
            .AsNoTracking()
            .Include(x => x.Mailboxes)
            .Where(x => x.Id == supplierId)
            .Where(x => x.TenantId == tenantId)
            .SingleOrDefaultAsync(cancellationToken);

        if (supplier is null)
        {
            _ctx.Error($"Supplier ID '{supplierId}' not found for tenant {tenantId})");
            return null;
        }

        _ctx.Success("Supplier loaded successfully");
        return supplier;
    }

    private async Task<string?> BuildEmailContentAsync(
        PartDefinitionsOrderViewModel order,
        SupplierViewModel supplier,
        CancellationToken cancellationToken)
    {
        _ctx.Step("Preparing email model...");

        var emailModel = MapToEmailModel(order, supplier);

        _ctx.Success("Email model prepared successfully");

        _ctx.Step("Rendering email template...");

        var emailContent = await templateRenderer.RenderAsync("PartDefinitionsOrder", emailModel, cancellationToken);

        if (string.IsNullOrWhiteSpace(emailContent))
        {
            _ctx.Error("Email template rendering returned empty content");
            return null;
        }

        _ctx.Success("Email template rendered successfully");
        return emailContent;
    }

    private async Task SendEmailsToSupplierAsync(
        PartDefinitionsOrderViewModel order,
        SupplierViewModel supplier,
        string emailContent,
        CancellationToken cancellationToken)
    {
        _ctx.Step("Sending email to supplier mailboxes...");

        var emails = supplier.Mailboxes
            .Select(m => m.Email)
            .Where(e => !string.IsNullOrWhiteSpace(e))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (emails.Count == 0)
        {
            _ctx.Error($"Supplier '{supplier.Name}' has no mailboxes configured");
        }

        _ctx.Step($"Sending separate emails to {emails.Count} recipient(s)...");

        var subject = $"Zamówienie formatek - {order.Number}";
        
        List<string> cc = settings.Value.IncludeCustomerEmailInCc ? [settings.Value.CustomerEmail] : [];

        foreach (var email in emails)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await emailService.SendEmailAsync(
                to: [email],
                subject: subject,
                body: emailContent,
                isHtml: true,
                cc: cc,
                cancellationToken: cancellationToken);

            _ctx.WriteLine($"  ✓ Email sent to {email}");
        }
    }

    private static PartDefinitionsOrderEmailModel MapToEmailModel(
        PartDefinitionsOrderViewModel order,
        SupplierViewModel supplier)
    {
        return new PartDefinitionsOrderEmailModel
        {
            OrderNumber = order.Number,
            OrderDate = order.CreatedAt,
            SupplierName = supplier.Name,
            AdditionalNotes = order.AdditionalNotes,
            FurnitureLines = order.FurnitureLines.Select(fl => new FurnitureLineEmailModel
            {
                FurnitureCode = fl.FurnitureCode,
                FurnitureVersion = fl.FurnitureVersion,
                QuantityOrdered = fl.QuantityOrdered,
                PartLines = fl.PartLines.Select(pl => new PartLineEmailModel
                {
                    Name = pl.Name,
                    LengthMm = pl.Dimensions.LengthMm,
                    WidthMm = pl.Dimensions.WidthMm,
                    ThicknessMm = pl.Dimensions.ThicknessMm,
                    LengthEdgeBanding = MapEdgeBanding(pl.Dimensions.LengthEdgeBandingType),
                    WidthEdgeBanding = MapEdgeBanding(pl.Dimensions.WidthEdgeBandingType),
                    Quantity = pl.Quantity,
                    TotalQuantity = pl.Quantity * fl.QuantityOrdered,
                    AdditionalInfo = pl.AdditionalInfo
                }).ToList()
            }).ToList()
        };
    }

    private static string MapEdgeBanding(OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel edgeBandingType)
    {
        return edgeBandingType switch
        {
            OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel.None => "Brak",
            OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel.One => "1x",
            OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel.Two => "2x",
            _ => "Nieznany"
        };
    }

}

