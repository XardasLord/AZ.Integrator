namespace AZ.Integrator.Infrastructure.ExternalServices.Allegro.Models;

internal class OriginalPrice
{
    public string Amount { get; set; }
    public string Currency { get; set; }
}

internal class Price
{
    public string Amount { get; set; }
    public string Currency { get; set; }
}

internal class External
{
    public string Id { get; set; }
}

internal class Offer
{
    public string Id { get; set; }
    public string Name { get; set; }
    public External External { get; set; }
}

internal class LineItem
{
    public string Id { get; set; }
    public Offer Offer { get; set; }
    public int Quantity { get; set; }
    public OriginalPrice OriginalPrice { get; set; }
    public Price Price { get; set; }
    public DateTime BoughtAt { get; set; }
}

internal class Preferences
{
    public string Language { get; set; }
}

internal class Buyer
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public bool Guest { get; set; }
    public Preferences Preferences { get; set; }
}

internal class Seller
{
    public string Id { get; set; }
}

internal class CheckoutForm
{
    public string Id { get; set; }
    public string Revision { get; set; }
}

internal class Marketplace
{
    public string Id { get; set; }
}

internal class Order
{
    public Seller Seller { get; set; }
    public Buyer Buyer { get; set; }
    public List<LineItem> LineItems { get; set; }
    public CheckoutForm CheckoutForm { get; set; }
    public Marketplace Marketplace { get; set; }
}

internal class Event
{
    public string Id { get; set; }
    public Order Order { get; set; }
    public string Type { get; set; }
    public DateTime OccurredAt { get; set; }
}

internal class GetOrderEventsModel
{
    public IEnumerable<Event> Events { get; set; }
}

internal class OrderTypes
{
    public const string Bought = "BOUGHT";
    public const string FilledIn = "FILLED_IN";
    public const string ReadyForProcessing = "READY_FOR_PROCESSING";
    public const string BuyerCanceller = "BUYER_CANCELLED";
    public const string FulfillmentStatusChanged = "FULFILLMENT_STATUS_CHANGED";
    public const string AutoCancelled = "AUTO_CANCELLED";
}