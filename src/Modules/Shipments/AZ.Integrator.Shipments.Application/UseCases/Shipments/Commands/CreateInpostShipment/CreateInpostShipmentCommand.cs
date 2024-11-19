using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX.Models;
using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.Commands.CreateInpostShipment;

public record CreateInpostShipmentCommand(
    Sender Sender,
    Receiver Receiver,
    List<Parcel> Parcels,
    Insurance Insurance,
    Cod Cod,
    string Reference,
    string Comments,
    string ExternalCustomerId,
    string ExternalOrderId) : IRequest<ShipmentResponse>;