namespace AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;

public class OriginalPrice
{
    public string Amount { get; set; }
    public string Currency { get; set; }
}

public class Price
{
    public string Amount { get; set; }
    public string Currency { get; set; }
}

public class External
{
    public string Id { get; set; }
}

public class Offer
{
    public string Id { get; set; }
    public string Name { get; set; }
    public External External { get; set; }
}

public class LineItem
{
    public string Id { get; set; }
    public Offer Offer { get; set; }
    public int Quantity { get; set; }
    public OriginalPrice OriginalPrice { get; set; }
    public Price Price { get; set; }
    public DateTime BoughtAt { get; set; }
}

public class Preferences
{
    public string Language { get; set; }
}

public class Buyer
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public bool Guest { get; set; }
    public Preferences Preferences { get; set; }
}

public class Seller
{
    public string Id { get; set; }
}

public class CheckoutForm
{
    public string Id { get; set; }
    public string Revision { get; set; }
}

public class Marketplace
{
    public string Id { get; set; }
}

public class Order
{
    public Seller Seller { get; set; }
    public Buyer Buyer { get; set; }
    public List<LineItem> LineItems { get; set; }
    public CheckoutForm CheckoutForm { get; set; }
    public Marketplace Marketplace { get; set; }
}

public class OrderEvent
{
    public string Id { get; set; }
    public Order Order { get; set; }
    public string Type { get; set; }
    public DateTime OccurredAt { get; set; }
}

public class GetOrderEventsModel
{
    public IEnumerable<OrderEvent> Events { get; set; }
}

public class OrderTypes
{
    public const string Bought = "BOUGHT";
    public const string FilledIn = "FILLED_IN";
    public const string ReadyForProcessing = "READY_FOR_PROCESSING";
    public const string BuyerCanceller = "BUYER_CANCELLED";
    public const string FulfillmentStatusChanged = "FULFILLMENT_STATUS_CHANGED";
    public const string AutoCancelled = "AUTO_CANCELLED";
}