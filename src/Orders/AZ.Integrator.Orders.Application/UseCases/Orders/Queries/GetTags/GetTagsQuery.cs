using MediatR;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetTags;

public record GetTagsQuery : IRequest<IEnumerable<string>>;