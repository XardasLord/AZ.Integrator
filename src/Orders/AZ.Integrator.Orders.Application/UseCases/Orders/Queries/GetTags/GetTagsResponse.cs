namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetTags;

public record GetTagsResponse(IEnumerable<string> Signatures, int TotalCount);