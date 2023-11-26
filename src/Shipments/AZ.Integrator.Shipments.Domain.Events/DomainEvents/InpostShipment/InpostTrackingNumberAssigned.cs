using MediatR;

namespace AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment;

public record InpostTrackingNumberAssigned(string ShipmentNumber, string TrackingNumber, string AllegroOrderNumber, string AllegroAccessToken) : INotification;