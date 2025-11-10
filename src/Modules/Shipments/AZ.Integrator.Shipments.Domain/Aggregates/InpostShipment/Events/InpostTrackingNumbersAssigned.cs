using Mediator;

namespace AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Events;

public record InpostTrackingNumbersAssigned(
    string ShipmentNumber,
    string[] TrackingNumbers,
    string ExternalOrderNumber,
    Guid TenantId,
    string SourceSystemId) : INotification;