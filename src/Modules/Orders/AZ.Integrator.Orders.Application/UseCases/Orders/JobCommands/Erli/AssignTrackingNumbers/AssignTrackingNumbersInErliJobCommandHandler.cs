using AZ.Integrator.Orders.Application.Common.ExternalServices.Erli;
using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Erli.AssignTrackingNumbers;

public class AssignTrackingNumbersInErliJobCommandHandler(IErliService erliService)
    : IRequestHandler<AssignTrackingNumbersInErliJobCommand>
{
    public async ValueTask<Unit> Handle(AssignTrackingNumbersInErliJobCommand command, CancellationToken cancellationToken)
    {
        command.PerformContext.Step("Starting to assign tracking numbers in Erli...");
        
        await erliService.AssignTrackingNumber(
            command.OrderNumber,
            command.TrackingNumbers,
            command.Vendor,
            command.DeliveryTrackingStatus,
            command.TenantId,
            command.SourceSystemId);
        
        command.PerformContext.Success("Tracking numbers assigned successfully in Erli.");
        
        return Unit.Value;
    }
}