using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetTags;

public record GetTagsQuery(GetProductTagsQueryFilters Filters) : IRequest<GetTagsResponse>;