using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using MediatR;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.ChangeAllegroOrderStatus;

public class ChangeAllegroOrderStatusJobCommandHandler : IRequestHandler<ChangeAllegroOrderStatusJobCommand>
{
    private readonly IAllegroService _allegroService;

    public ChangeAllegroOrderStatusJobCommandHandler(IAllegroService allegroService)
    {
        _allegroService = allegroService;
    }
    
    public async Task<Unit> Handle(ChangeAllegroOrderStatusJobCommand command, CancellationToken cancellationToken)
    {
        await _allegroService.ChangeStatus(command.OrderNumber, command.OrderStatus, command.AllegroAccessToken);
        
        return Unit.Value;
    }
}