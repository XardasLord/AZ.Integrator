using AZ.Integrator.Application.Common.ExternalServices.ShipX.Models;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Shipments.Commands.CreateInpostShipment;

public record CreateInpostShipmentCommand(
    Sender Sender,
    Receiver Receiver,
    List<Parcel> Parcels,
    Insurance Insurance,
    Cod Cod,
    string Reference,
    string Comments,
    string ExternalCustomerId) : ICommand<ShipmentResponse>;