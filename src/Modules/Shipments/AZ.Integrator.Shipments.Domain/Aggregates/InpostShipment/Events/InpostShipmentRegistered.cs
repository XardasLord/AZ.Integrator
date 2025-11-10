using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel;

namespace AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Events;

public record InpostShipmentRegistered(
    string ShipmentNumber,
    string ExternalOrderNumber,
    string SourceSystemId,
    Guid TenantId,
    ShopProviderType ShopProviderType,
    string CorrelationId) : ITrackableNotification;