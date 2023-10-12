using AZ.Integrator.Application.UseCases.Shipments.JobCommands.SetInpostTrackingNumber;
using Hangfire;
using Mediator;
using InpostShipmentRegisteredEvent = AZ.Integrator.Domain.Aggregates.InpostShipment.DomainEvents.InpostShipmentRegistered;

namespace AZ.Integrator.Application.UseCases.Shipments.EventHandlers.InpostShipmentRegistered;

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
                ShippingNumber = notification.ShipmentNumber
            }, null));
        
        return ValueTask.CompletedTask;
    }
}