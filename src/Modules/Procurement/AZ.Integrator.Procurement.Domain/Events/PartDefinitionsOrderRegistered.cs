using Mediator;

namespace AZ.Integrator.Procurement.Domain.Events;

public sealed record PartDefinitionsOrderRegistered(
    string OrderNumber,
    uint SupplierId,
    Guid TenantId) : INotification;

