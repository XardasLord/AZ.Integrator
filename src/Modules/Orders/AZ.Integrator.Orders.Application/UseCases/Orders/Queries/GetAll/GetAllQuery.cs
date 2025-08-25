using AZ.Integrator.Shared.Application;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetAll;

public record GetAllQuery(GetAllQueryFilters Filters) : HeaderRequest, IRequest<GetAllQueryResponse>;