using MediatR;

namespace AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment;

public record InpostTrackingNumberAssigned(string ShipmentNumber, string TrackingNumber) : INotification;