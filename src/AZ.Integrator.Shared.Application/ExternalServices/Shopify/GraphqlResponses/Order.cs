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
    public string Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public string Email { get; set; }
    public bool FullyPaid { get; set; }
    public string Note { get; set; }
    public bool BillingAddressMatchesShippingAddress { get; set; }
    public TotalPriceSet TotalPriceSet { get; set; }
    public ShippingLine ShippingLine { get; set; }
    public ShippingAddress ShippingAddress { get; set; }
    public ShippingAddress BillingAddress { get; set; }
    public LineItemConnection LineItems { get; set; }
    public FulfillmentOrderConnection FulfillmentOrders { get; set; }
    public string DisplayFinancialStatus { get; set; }
    public string DisplayFulfillmentStatus { get; set; }
}

public class TotalPriceSet
{
    public Money PresentmentMoney { get; set; }
    public Money ShopMoney { get; set; }
}

public class Money
{
    public string Amount { get; set; }
    public string CurrencyCode { get; set; }
}

public class ShippingLine
{
    public string Title { get; set; }
    public TotalPriceSet CurrentDiscountedPriceSet { get; set; }
    public TotalPriceSet OriginalPriceSet { get; set; }
}

public class ShippingAddress
{
    public string Company { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string Country { get; set; }
    public string Zip { get; set; }
    public string Phone { get; set; }
    public string CountryCodeV2 { get; set; }
}

public class LineItemConnection
{
    public List<LineItem> Nodes { get; set; }
    public PageInfo PageInfo { get; set; }
}

public class LineItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string Sku { get; set; }
    public int Quantity { get; set; }
    public TotalPriceSet OriginalUnitPriceSet { get; set; }
    public Product Product { get; set; }
}

public class Product
{
    public string Id { get; set; }
    public string Description { get; set; }
}

public class FulfillmentOrderConnection
{
    public List<FulfillmentOrder> Nodes { get; set; }
    public PageInfo PageInfo { get; set; }
}

public class FulfillmentOrder
{
    public string Id { get; set; }
    public string Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}