using AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.AssignTrackingNumbers;
using AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment;
using Hangfire;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.EventHandlers.InpostTrackingNumberAssigned;

public class AssignTrackingNumberInAllegro : INotificationHandler<InpostTrackingNumbersAssigned>
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public AssignTrackingNumberInAllegro(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }
    
    public ValueTask Handle(InpostTrackingNumbersAssigned notification, CancellationToken cancellationToken)
    {
        _backgroundJobClient.Enqueue<AssignTrackingNumberJob>(
            job => job.Execute(new AssignTrackingNumbersJobCommand
            {
                OrderNumber = Guid.Parse(notification.AllegroOrderNumber),
                TrackingNumbers = notification.TrackingNumbers,
                TenantId = notification.TenantId
            }, null));
        
        return new ValueTask();
    }
}