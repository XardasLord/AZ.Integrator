using AZ.Integrator.Procurement.Application.Common.BackgroundJobs;
using AZ.Integrator.Procurement.Contracts.PartDefinitionOrders;
using AZ.Integrator.Procurement.Contracts.Suppliers;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.JobCommands;

public class SendPartDefinitionOrderEmailToSupplierJobCommandHandler(
    IProcurementDataViewContext dataViewContext,
    IEmailService emailService)
    : IRequestHandler<SendPartDefinitionOrderEmailToSupplierJobCommand>
{
    public async ValueTask<Unit> Handle(SendPartDefinitionOrderEmailToSupplierJobCommand command, CancellationToken cancellationToken)
    {
        var ctx = command.PerformContext;
        
        ctx.Step($"Starting loading order '{command.OrderNumber}' for tenant {command.TenantId}...");
        
        var order = await dataViewContext.Orders
            .Include(x => x.FurnitureLines)
            .ThenInclude(x => x.PartLines)
            .Where(x => x.Number == command.OrderNumber)
            .Where(x => x.TenantId == command.TenantId)
            .SingleOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            ctx.Error($"Order '{command.OrderNumber}' not found for tenant {command.TenantId}");
            return Unit.Value;
        }
        
        ctx.Success("Order loaded successfully");
        
        ctx.Step($"Starting loading supplier ID '{order.SupplierId}' for tenant {command.TenantId}...");
        
        var supplier = await dataViewContext.Suppliers
            .Include(x => x.Mailboxes)
            .Where(x => x.Id == order.SupplierId)
            .Where(x => x.TenantId == command.TenantId)
            .SingleOrDefaultAsync(cancellationToken);

        if (supplier is null)
        {
            ctx.Error($"Supplier ID '{order.SupplierId}' not found for tenant {command.TenantId})");
            return Unit.Value;
        }
        
        ctx.Success("Supplier loaded successfully");
        
        ctx.Step("Generating email content...");
        
        var emailContent = GenerateEmailContent(order, supplier);
        
        ctx.Success("Email content generated successfully");
            
        ctx.Step("Sending email to supplier mailboxes...");
        
        // 4. Send email to all supplier mailboxes
        var emails = supplier.Mailboxes.Select(m => m.Email).ToList();
        await emailService.SendEmailAsync(
            to: emails,
            subject: $"Zamówienie formatek - {order.Number}",
            body: emailContent,
            isHtml: true,
            cancellationToken: cancellationToken);
        
        ctx.Success("Email sent successfully to supplier mailboxes");
        
        return Unit.Value;
    }
    
    private string GenerateEmailContent(PartDefinitionsOrderViewModel order, SupplierViewModel supplier)
    {
        // TODO: Use a templating engine (e.g., Razor, Scriban) to generate HTML email
        // Include:
        // - Order number
        // - Order date
        // - List of furniture models with quantities
        // - Part definitions with dimensions, quantities, edge banding
        // - Additional information
        // - Contact information
        
        return $"<html>...</html>";
    }
}