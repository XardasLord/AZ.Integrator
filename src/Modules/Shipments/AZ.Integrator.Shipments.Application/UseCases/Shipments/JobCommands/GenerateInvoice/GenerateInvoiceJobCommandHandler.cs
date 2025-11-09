using AZ.Integrator.Domain.Extensions;
using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder;
using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.GenerateInvoice;

public class GenerateInvoiceJobCommandHandler(IMediator mediator) : IRequestHandler<GenerateInvoiceJobCommand>
{
    public async ValueTask<Unit> Handle(GenerateInvoiceJobCommand command, CancellationToken cancellationToken)
    {
        command.CorrelationId ??= CorrelationIdHelper.New();
        
        var ctx = command.PerformContext;

        if (command.ShopProvider == ShopProviderType.Shopify)
        {
            ctx.Warning("Current plan of Shopify is not ready to obtain customer details, so Invoice generation is skipped.'");
            return Unit.Value;
        }
        
        ctx.Step($"Starting generating invoice for order - '{command.ExternalOrderNumber}'");
        
        var commandRequest = new GenerateInvoiceForOrderCommand(command.ExternalOrderNumber, command.CorrelationId)
        {
            TenantId = command.TenantId,
            SourceSystemId = command.SourceSystemId,
            ShopProvider = command.ShopProvider
        };

        ctx.Info("Generating invoice file...");
        
        var invoiceResponse = await mediator.Send(commandRequest, cancellationToken);
        
        ctx.Success($"Invoice generated successfully (InvoiceNumber: {invoiceResponse?.Number}, ID: {invoiceResponse?.Id})");
        
        return Unit.Value;
    }
}