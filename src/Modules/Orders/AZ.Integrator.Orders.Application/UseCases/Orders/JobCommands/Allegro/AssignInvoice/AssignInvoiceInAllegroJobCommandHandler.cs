using AZ.Integrator.Invoices.Contracts;
using AZ.Integrator.Invoices.Contracts.Dtos;
using AZ.Integrator.Orders.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Shared.Application;
using Mediator;
using Microsoft.Extensions.Configuration;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.AssignInvoice;

public class AssignInvoiceInAllegroJobCommandHandler(
    IInvoicesFacade invoiceFacade,
    IAllegroService allegroService,
    IConfiguration configuration)
    : IRequestHandler<AssignInvoiceInAllegroJobCommand>
{
    public async ValueTask<Unit> Handle(AssignInvoiceInAllegroJobCommand command, CancellationToken cancellationToken)
    {
        var ctx = command.PerformContext;
        
        var enabled = configuration.GetValue<bool>("Infrastructure:Integrations:Flows:AssignInvoiceInAllegro:Enabled");
        if (!enabled)
        {
            ctx.Info("Automatic invoice assignment is disabled");
            return Unit.Value;
        }
        
        ctx.Step($"Starting assignment of invoice '{command.InvoiceNumber}' to Allegro order {command.OrderNumber}");

        var request = new GetInvoiceRequest(command.ExternalInvoiceId, command.OrderNumber.ToString(),
            command.InvoiceProvider, command.TenantId, command.SourceSystemId);
            
        ctx.Info("Fetching invoice file from Invoice Module...");
        
        var invoiceResponse = await invoiceFacade.GetInvoice(request, cancellationToken);
        
        ctx.Success($"Invoice file retrieved successfully (InvoiceNumber: {invoiceResponse.InvoiceNumber})");
        ctx.Step($"Uploading invoice '{invoiceResponse.InvoiceNumber}' to Allegro order {command.OrderNumber}");
        
        await allegroService.AssignInvoice(
            command.OrderNumber,
            invoiceResponse.File,
            invoiceResponse.InvoiceNumber,
            command.TenantId,
            command.SourceSystemId);
        
        ctx.Success("Invoice file successfully uploaded to Allegro order");
        
        return Unit.Value;
    }
}