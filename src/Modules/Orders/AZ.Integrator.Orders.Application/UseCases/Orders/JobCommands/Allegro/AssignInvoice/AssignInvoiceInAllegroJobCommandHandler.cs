using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.AssignInvoice;

public class AssignInvoiceInAllegroJobCommandHandler(IAllegroService allegroService)
    : IRequestHandler<AssignInvoiceInAllegroJobCommand>
{
    public ValueTask<Unit> Handle(AssignInvoiceInAllegroJobCommand command, CancellationToken cancellationToken)
    {
        // await allegroService.AssignTrackingNumber(command.OrderNumber, command.TrackingNumbers, command.TenantId);
        
        return ValueTask.FromResult(Unit.Value);
    }
}