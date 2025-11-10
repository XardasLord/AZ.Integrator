using AZ.Integrator.Shipments.Contracts.IntegrationEvents;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Events;
using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.EventHandlers;

public class InpostTrackingNumbersAssignedHandler(IMediator mediator) 
    : INotificationHandler<InpostTrackingNumbersAssigned>
{
    public async ValueTask Handle(InpostTrackingNumbersAssigned notification, CancellationToken cancellationToken)
    {
        // In the future, other details can be added to the event
        var @event = new InpostTrackingNumbersAssignedV1(
            notification.ShipmentNumber,
            notification.TrackingNumbers,
            notification.ExternalOrderNumber,
            notification.TenantId,
            notification.SourceSystemId);

        await mediator.Publish(@event, cancellationToken);
    }
}