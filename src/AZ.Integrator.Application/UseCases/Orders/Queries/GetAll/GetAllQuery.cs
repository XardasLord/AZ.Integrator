using Mediator;

namespace AZ.Integrator.Application.UseCases.Orders.Queries.GetAll;

public record GetAllQuery(GetAllQueryFilters Filters) : IQuery<GetAllQueryResponse>;