using AZ.Integrator.Application.Common.ExternalServices.ShipX.Models;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Shipments.Commands;

public record CreateInpostShipmentCommand() : ICommand<ShipmentResponse>;