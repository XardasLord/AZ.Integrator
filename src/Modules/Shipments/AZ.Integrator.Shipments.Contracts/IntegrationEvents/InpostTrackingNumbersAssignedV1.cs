using Mediator;

namespace AZ.Integrator.Shipments.Contracts.IntegrationEvents;

public record InpostTrackingNumbersAssignedV1(
    string ShipmentNumber,
    string[] TrackingNumbers,
    string ExternalOrderNumber,
    Guid TenantId,
    string SourceSystemId) : INotification;