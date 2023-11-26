using MediatR;

namespace AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment;

public record InpostShipmentRegistered(string ShipmentNumber, string AllegroOrderNumber, string AllegroAccessToken) : INotification;