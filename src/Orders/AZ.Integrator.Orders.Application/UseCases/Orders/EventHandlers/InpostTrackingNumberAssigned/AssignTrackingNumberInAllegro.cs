using AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.AssignTrackingNumbers;
using Hangfire;
using MediatR;
using InpostTrackingNumberAssignedEvent = AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment.InpostTrackingNumberAssigned;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.EventHandlers.InpostTrackingNumberAssigned;

public class AssignTrackingNumberInAllegro : INotificationHandler<InpostTrackingNumberAssignedEvent>
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public AssignTrackingNumberInAllegro(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }
    
    public Task Handle(InpostTrackingNumberAssignedEvent notification, CancellationToken cancellationToken)
    {
        _backgroundJobClient.Enqueue<AssignTrackingNumberJob>(
            job => job.Execute(new AssignTrackingNumberJobCommand
            {
                OrderNumber = Guid.Parse(notification.AllegroOrderNumber),
                TrackingNumber = notification.TrackingNumber,
                TenantId = notification.TenantId
            }, null));
        
        return Task.CompletedTask;
    }
}