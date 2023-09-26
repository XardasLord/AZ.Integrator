using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Orders.Queries.GetDetails;

public record GetDetailsQuery(Guid OrderId) : IQuery<OrderDetailsDto>;