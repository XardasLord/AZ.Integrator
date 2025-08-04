namespace AZ.Integrator.Shared.Application.ExternalServices.Shopify.GraphqlResponses;

public class GetOrdersCountResponse
{
    public OrdersCountNode OrdersCount { get; set; }
}

public class OrdersCountNode
{
    public int Count { get; set; }
    public string Precision { get; set; }
}