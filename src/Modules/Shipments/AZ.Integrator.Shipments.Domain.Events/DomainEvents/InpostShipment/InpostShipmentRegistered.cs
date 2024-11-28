using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using Mediator;

namespace AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment;

public record InpostShipmentRegistered(string ShipmentNumber, string ExternalOrderNumber, TenantId TenantId) : INotification;