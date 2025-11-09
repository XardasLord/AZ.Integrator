using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using Mediator;

namespace AZ.Integrator.Shipments.Domain.Events.DomainEvents.DpdShipment;

public record DpdShipmentRegistered(
    long SessionNumber,
    string AllegroOrderNumber,
    SourceSystemId SourceSystemId,
    TenantId TenantId,
    ShopProviderType ShopProviderType,
    string CorrelationId) : INotification;