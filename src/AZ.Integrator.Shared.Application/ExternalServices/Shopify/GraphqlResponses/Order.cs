namespace AZ.Integrator.Shared.Application.ExternalServices.Shopify.GraphqlResponses;

public class GetOrdersResponse
{
    public OrdersConnection Orders { get; set; }
}

public class OrdersConnection
{
    public List<OrderEdge> Edges { get; set; }
    public PageInfo PageInfo { get; set; }
}

public class OrderEdge
{
    public string Cursor { get; set; }
    public Order Node { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public List<FulfillmentOrder> FulfillmentOrders { get; set; }
    // TODO: Add other order properties as needed.
}

public class FulfillmentOrder
{
    public string Id { get; set; }
    public string OrderId { get; set; }
    public string Status { get; set; }
}