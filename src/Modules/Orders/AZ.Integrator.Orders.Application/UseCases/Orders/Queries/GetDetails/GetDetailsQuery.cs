using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetDetails;

public record GetDetailsQuery(Guid OrderId) : IRequest<OrderDetailsDto>;