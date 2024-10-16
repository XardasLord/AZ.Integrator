using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetAll;

public record GetAllQuery(GetAllQueryFilters Filters) : IRequest<GetAllQueryResponse>;