using AZ.Integrator.Shared.Application.ExternalServices.Shopify.GraphqlResponses;

namespace AZ.Integrator.Shared.Application.ExternalServices.Shopify;

public class GetOrdersModelResponse
{
    public IEnumerable<Order> Orders { get; set; }
    public int Count { get; set; }
    public int TotalCount { get; set; }
}