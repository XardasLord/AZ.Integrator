using AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.SetInpostTrackingNumber;
using Hangfire;
using Mediator;
using InpostShipmentRegisteredEvent = AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment.InpostShipmentRegistered;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.EventHandlers.InpostShipmentRegistered;

public class SetTrackingNumber : INotificationHandler<InpostShipmentRegisteredEvent>
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public SetTrackingNumber(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }
    
    public ValueTask Handle(InpostShipmentRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _backgroundJobClient.Enqueue<SetInpostTrackingNumberJob>(
            job => job.Execute(new SetInpostTrackingNumberJobCommand
            {
                ShippingNumber = notification.ShipmentNumber,
                TenantId = notification.TenantId
            }, null));
        
        return new ValueTask();
    }
}