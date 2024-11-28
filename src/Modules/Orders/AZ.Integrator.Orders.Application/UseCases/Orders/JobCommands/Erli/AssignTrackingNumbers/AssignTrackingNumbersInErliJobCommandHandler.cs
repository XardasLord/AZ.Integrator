using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Erli;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Erli.AssignTrackingNumbers;

public class AssignTrackingNumbersInErliJobCommandHandler(IErliService erliService)
    : IRequestHandler<AssignTrackingNumbersInErliJobCommand>
{
    public async ValueTask<Unit> Handle(AssignTrackingNumbersInErliJobCommand command, CancellationToken cancellationToken)
    {
        await erliService.AssignTrackingNumber(
            command.OrderNumber,
            command.TrackingNumbers,
            command.Vendor,
            command.DeliveryTrackingStatus,
            command.TenantId);
        
        return Unit.Value;
    }
}