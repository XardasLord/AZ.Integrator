using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Orders.Commands.GetDetails;

public record GetDetailsCommand(Guid OrderId) : ICommand<OrderDetailsDto>;