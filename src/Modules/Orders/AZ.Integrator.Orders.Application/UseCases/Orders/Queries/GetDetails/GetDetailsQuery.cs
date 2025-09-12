using AZ.Integrator.Orders.Contracts.Dtos;
using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetDetails;

public record GetDetailsQuery(string OrderId) : HeaderRequest, IRequest<OrderDetailsDto>;