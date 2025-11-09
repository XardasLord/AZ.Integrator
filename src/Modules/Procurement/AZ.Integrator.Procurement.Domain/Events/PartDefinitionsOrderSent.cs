using Mediator;

namespace AZ.Integrator.Procurement.Domain.Events;

/// <summary>
/// Domain event raised when a part definitions order is marked as sent.
/// This event triggers the email sending process to the supplier.
/// </summary>
public sealed record PartDefinitionsOrderSent(
    uint OrderId,
    string OrderNumber,
    uint SupplierId,
    Guid TenantId) : INotification;

