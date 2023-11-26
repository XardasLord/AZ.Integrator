using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using MediatR;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.AssignShipmentNumbers;

public class ChangeStatusJobCommandHandler : MediatR.IRequestHandler<AssignShipmentNumbersJobCommand>
{
    private readonly IAllegroService _allegroService;

    public ChangeStatusJobCommandHandler(IAllegroService allegroService)
    {
        _allegroService = allegroService;
    }
    
    public async Task<Unit> Handle(AssignShipmentNumbersJobCommand command, CancellationToken cancellationToken)
    {
        await _allegroService.ChangeStatus(command.OrderNumber, AllegroFulfillmentStatusEnum.ReadyForShipment, command.AllegroAccessToken);
        
        return Unit.Value;
    }
}