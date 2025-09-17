using AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder;
using Hangfire.Console;
using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.GenerateInvoice;

public class GenerateInvoiceJobCommandHandler(IMediator mediator) : IRequestHandler<GenerateInvoiceJobCommand>
{
    public async ValueTask<Unit> Handle(GenerateInvoiceJobCommand command, CancellationToken cancellationToken)
    {
        command.PerformContext.WriteLine($"Starting generating invoice for order - {command.ExternalOrderNumber}");
        
        var commandRequest = new GenerateInvoiceForOrderCommand(command.ExternalOrderNumber)
        {
            TenantId = command.TenantId,
            ShopProvider = command.ShopProvider
        };

        var invoiceResponse = await mediator.Send(commandRequest, cancellationToken);
        
        command.PerformContext.SetTextColor(ConsoleTextColor.DarkGreen);
        command.PerformContext.WriteLine($"Invoice generated - '{invoiceResponse?.Number}', with ID - '{invoiceResponse?.Id}'");
        
        return Unit.Value;
    }
}