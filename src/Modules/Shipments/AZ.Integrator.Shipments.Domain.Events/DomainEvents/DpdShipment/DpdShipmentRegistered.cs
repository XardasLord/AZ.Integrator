using Mediator;

namespace AZ.Integrator.Shipments.Domain.Events.DomainEvents.DpdShipment;

public record DpdShipmentRegistered(long SessionNumber, string AllegroOrderNumber) : INotification;