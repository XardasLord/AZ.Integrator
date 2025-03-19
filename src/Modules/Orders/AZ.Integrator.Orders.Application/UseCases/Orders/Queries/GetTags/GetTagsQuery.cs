using AZ.Integrator.Shared.Application;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetTags;

public record GetTagsQuery(GetProductTagsQueryFilters Filters) : HeaderRequest, IRequest<GetTagsResponse>;