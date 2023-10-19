using AZ.Integrator.Domain.Aggregates.DpdShipment.ValueObjects;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using Mediator;

namespace AZ.Integrator.Domain.Aggregates.DpdShipment.DomainEvents;

public record DpdShipmentRegistered(SessionNumber SessionNumber, AllegroOrderNumber AllegroOrderNumber) : INotification;