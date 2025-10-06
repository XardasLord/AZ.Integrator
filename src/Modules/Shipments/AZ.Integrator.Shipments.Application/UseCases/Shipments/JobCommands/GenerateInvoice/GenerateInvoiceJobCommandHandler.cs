using AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder;
using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.GenerateInvoice;

public class GenerateInvoiceJobCommandHandler(IMediator mediator) : IRequestHandler<GenerateInvoiceJobCommand>
{
    public async ValueTask<Unit> Handle(GenerateInvoiceJobCommand command, CancellationToken cancellationToken)
    {
        var ctx = command.PerformContext;
        
        ctx.Step($"Starting generating invoice for order - '{command.ExternalOrderNumber}'");
        
        var commandRequest = new GenerateInvoiceForOrderCommand(command.ExternalOrderNumber)
        {
            TenantId = command.TenantId,
            ShopProvider = command.ShopProvider
        };

        ctx.Info("Generating invoice file...");
        
        var invoiceResponse = await mediator.Send(commandRequest, cancellationToken);
        
        ctx.Success($"Invoice generated successfully (InvoiceNumber: {invoiceResponse?.Number}, ID: {invoiceResponse?.Id})");
        
        return Unit.Value;
    }
}