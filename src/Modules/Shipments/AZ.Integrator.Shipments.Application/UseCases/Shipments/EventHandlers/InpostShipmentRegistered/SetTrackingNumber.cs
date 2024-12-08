using AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.SetInpostTrackingNumber;
using Hangfire;
using Mediator;
using InpostShipmentRegisteredEvent = AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment.InpostShipmentRegistered;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.EventHandlers.InpostShipmentRegistered;

public class SetTrackingNumber(IBackgroundJobClient backgroundJobClient) 
    : INotificationHandler<InpostShipmentRegisteredEvent>
{
    public ValueTask Handle(InpostShipmentRegisteredEvent notification, CancellationToken cancellationToken)
    {
        backgroundJobClient.Enqueue<SetInpostTrackingNumberJob>(
            job => job.Execute(new SetInpostTrackingNumberJobCommand
            {
                ShippingNumber = notification.ShipmentNumber,
                ExternalOrderNumber = notification.ExternalOrderNumber,
                TenantId = notification.TenantId
            }, null));
        
        return new ValueTask();
    }
}