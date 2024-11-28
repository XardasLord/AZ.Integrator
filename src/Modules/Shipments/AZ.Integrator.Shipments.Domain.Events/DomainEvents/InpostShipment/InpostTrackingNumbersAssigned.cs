using Mediator;

namespace AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment;

public record InpostTrackingNumbersAssigned(
    string ShipmentNumber,
    string[] TrackingNumbers,
    string ExternalOrderNumber,
    string TenantId) : INotification;