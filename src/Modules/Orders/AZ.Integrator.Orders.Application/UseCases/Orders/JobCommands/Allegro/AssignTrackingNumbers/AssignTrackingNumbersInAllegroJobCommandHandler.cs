using AZ.Integrator.Orders.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.AssignTrackingNumbers;

public class AssignTrackingNumbersInAllegroJobCommandHandler(IAllegroService allegroService)
    : IRequestHandler<AssignTrackingNumbersInAllegroJobCommand>
{
    public async ValueTask<Unit> Handle(AssignTrackingNumbersInAllegroJobCommand command, CancellationToken cancellationToken)
    {
        command.PerformContext.Step("Starting assigning tracking numbers in Allegro...");
        
        await allegroService.AssignTrackingNumber(
            command.OrderNumber,
            command.TrackingNumbers,
            command.TenantId,
            command.SourceSystemId);
        
        command.PerformContext.Success("Tracking numbers assigned successfully in Allegro");
        
        return Unit.Value;
    }
}