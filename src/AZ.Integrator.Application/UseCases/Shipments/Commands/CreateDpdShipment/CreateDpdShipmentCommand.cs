using AZ.Integrator.Application.Common.ExternalServices.ShipX.Models;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Shipments.Commands.CreateDpdShipment;

public record CreateDpdShipmentCommand(
    Sender Sender,
    Receiver Receiver,
    List<Parcel> Parcels,
    Insurance Insurance,
    Cod Cod,
    string Reference,
    string Comments,
    string ExternalCustomerId) : ICommand<object>;