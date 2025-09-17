namespace AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;

public sealed record GetAllQueryFilters
{
    public string OrderFulfillmentStatus { get; init; }
    public int Take { get; init; }
    public int Skip { get; init; }
    public string SearchText { get; init; }
}