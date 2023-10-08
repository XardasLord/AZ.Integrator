using AZ.Integrator.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Orders.JobCommands.ChangeStatus;

public class ChangeStatusJobCommandHandler : ICommandHandler<ChangeStatusJobCommand>
{
    private readonly IAllegroService _allegroService;

    public ChangeStatusJobCommandHandler(IAllegroService allegroService)
    {
        _allegroService = allegroService;
    }
    
    public async ValueTask<Unit> Handle(ChangeStatusJobCommand command, CancellationToken cancellationToken)
    {
        await _allegroService.ChangeStatus(command.OrderNumber, AllegroStatusEnum.ReadyForShipment, command.AllegroAccessToken);
        
        return Unit.Value;
    }
}