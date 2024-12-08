using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.ChangeOrderStatus;

public class ChangeAllegroOrderStatusJobCommandHandler(IAllegroService allegroService)
    : IRequestHandler<ChangeAllegroOrderStatusJobCommand>
{
    public async ValueTask<Unit> Handle(ChangeAllegroOrderStatusJobCommand command, CancellationToken cancellationToken)
    {
        await allegroService.ChangeStatus(command.OrderNumber, AllegroFulfillmentStatusEnum.FromValue(command.OrderStatus), command.TenantId);
        
        return Unit.Value;
    }
}