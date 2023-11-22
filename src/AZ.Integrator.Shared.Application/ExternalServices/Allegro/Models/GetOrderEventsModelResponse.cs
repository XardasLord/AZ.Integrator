using Ardalis.SmartEnum;

namespace AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

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

public class GetOrderEventsModelResponse
{
    public IEnumerable<OrderEvent> Events { get; set; }
}

public sealed class AllegroOrderTypesEnum : SmartEnum<AllegroOrderTypesEnum>
{
    public static readonly AllegroOrderTypesEnum Bought = new("BOUGHT", 0);
    public static readonly AllegroOrderTypesEnum FilledIn = new("FILLED_IN", 1);
    public static readonly AllegroOrderTypesEnum ReadyForProcessing = new("READY_FOR_PROCESSING", 2);
    public static readonly AllegroOrderTypesEnum BuyerCanceller = new("BUYER_CANCELLED", 3);
    public static readonly AllegroOrderTypesEnum FulfillmentStatusChanged = new("FULFILLMENT_STATUS_CHANGED", 4);
    public static readonly AllegroOrderTypesEnum AutoCancelled = new("AUTO_CANCELLED", 5);
    
    private AllegroOrderTypesEnum(string name, int value) : base(name, value)
    {
    }
}