using AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.SetInpostTrackingNumber;
using Hangfire;
using MediatR;
using InpostShipmentRegisteredEvent = AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment.InpostShipmentRegistered;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.EventHandlers.InpostShipmentRegistered;

public class SetTrackingNumber : INotificationHandler<InpostShipmentRegisteredEvent>
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public SetTrackingNumber(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }
    
    public Task Handle(InpostShipmentRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _backgroundJobClient.Enqueue<SetInpostTrackingNumberJob>(
            job => job.Execute(new SetInpostTrackingNumberJobCommand
            {
                ShippingNumber = notification.ShipmentNumber,
                AllegroAccessToken = notification.AllegroAccessToken
            }, null));
        
        return Task.CompletedTask;
    }
}