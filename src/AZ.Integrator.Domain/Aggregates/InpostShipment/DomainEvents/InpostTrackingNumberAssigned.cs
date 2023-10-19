using AZ.Integrator.Domain.Aggregates.InpostShipment.ValueObjects;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using Mediator;

namespace AZ.Integrator.Domain.Aggregates.InpostShipment.DomainEvents;

public record InpostTrackingNumberAssigned(ShipmentNumber ShipmentNumber, TrackingNumber TrackingNumber) : INotification;