using AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder;
using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.GenerateInvoice;

public class GenerateInvoiceJobCommandHandler(IMediator mediator) : IRequestHandler<GenerateInvoiceJobCommand>
{
    public async ValueTask<Unit> Handle(GenerateInvoiceJobCommand command, CancellationToken cancellationToken)
    {
        var commandRequest = new GenerateInvoiceForOrderCommand(command.ExternalOrderNumber)
        {
            TenantId = command.TenantId,
            ShopProvider = command.ShopProvider
        };

        await mediator.Send(commandRequest, cancellationToken);
        
        return Unit.Value;
    }
}