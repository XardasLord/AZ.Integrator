using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using MediatR;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetDetails;

public record GetDetailsQuery(Guid OrderId) : IRequest<OrderDetailsDto>;