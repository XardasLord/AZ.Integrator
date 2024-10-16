using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.ChangeAllegroOrderStatus;

public class ChangeAllegroOrderStatusJobCommandHandler : IRequestHandler<ChangeAllegroOrderStatusJobCommand>
{
    private readonly IAllegroService _allegroService;

    public ChangeAllegroOrderStatusJobCommandHandler(IAllegroService allegroService)
    {
        _allegroService = allegroService;
    }
    
    public async ValueTask<Unit> Handle(ChangeAllegroOrderStatusJobCommand command, CancellationToken cancellationToken)
    {
        await _allegroService.ChangeStatus(command.OrderNumber, AllegroFulfillmentStatusEnum.FromValue(command.OrderStatus), command.TenantId);
        
        return Unit.Value;
    }
}