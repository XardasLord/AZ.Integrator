using AZ.Integrator.Domain.Aggregates.InpostShipment.ValueObjects;
using Mediator;

namespace AZ.Integrator.Domain.Aggregates.InpostShipment.DomainEvents;

public record InpostShipmentRegistered(ShipmentNumber ShipmentNumber, AllegroOrderNumber AllegroOrderNumber) : INotification;