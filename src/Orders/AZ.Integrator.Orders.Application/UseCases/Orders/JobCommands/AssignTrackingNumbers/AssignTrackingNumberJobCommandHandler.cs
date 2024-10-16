using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.AssignTrackingNumbers;

public class AssignTrackingNumberJobCommandHandler : IRequestHandler<AssignTrackingNumbersJobCommand>
{
    private readonly IAllegroService _allegroService;

    public AssignTrackingNumberJobCommandHandler(IAllegroService allegroService)
    {
        _allegroService = allegroService;
    }
    
    public async ValueTask<Unit> Handle(AssignTrackingNumbersJobCommand command, CancellationToken cancellationToken)
    {
        await _allegroService.AssignTrackingNumber(command.OrderNumber, command.TrackingNumbers, command.TenantId);
        
        return Unit.Value;
    }
}