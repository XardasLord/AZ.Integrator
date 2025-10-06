using AZ.Integrator.Domain.SharedKernel;
using Mediator;

namespace AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment;

public record InpostShipmentRegistered(
    string ShipmentNumber,
    string ExternalOrderNumber,
    string SourceSystemId,
    string TenantId,
    ShopProviderType ShopProviderType,
    string CorrelationId) : INotification;