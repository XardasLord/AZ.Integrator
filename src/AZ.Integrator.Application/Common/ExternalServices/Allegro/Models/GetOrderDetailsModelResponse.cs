namespace AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;

public class GetOrderDetailsModelResponse
{
    public string Id { get; set; }
    public string MessageToSeller { get; set; }
    public BuyerDetails Buyer { get; set; }
    public PaymentDetails Payment { get; set; }
    public string Status { get; set; }
    public FulfillmentDetails Fulfillment { get; set; }
    public DeliveryDetails Delivery { get; set; }
    // public InvoiceDetails Invoice { get; set; }
    public List<LineItemDetails> LineItems { get; set; }
    public List<SurchargeDetails> Surcharges { get; set; }
    public List<DiscountDetails> Discounts { get; set; }
    public NoteDetails Note { get; set; }
    public MarketplaceDetails Marketplace { get; set; }
    public SummaryDetails Summary { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Revision { get; set; }
}

public class BuyerDetails
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public object CompanyName { get; set; }
    public bool Guest { get; set; }
    public string PersonalIdentity { get; set; }
    public object PhoneNumber { get; set; }
    public PreferencesDetails Preferences { get; set; }
    public AddressDetails Address { get; set; }
}

public class PreferencesDetails
{
    public string Language { get; set; }
}

public class AddressDetails
{
    public string Street { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
    public string CountryCode { get; set; }
}

public class PaymentDetails
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Provider { get; set; }
    public DateTime FinishedAt { get; set; }
    public AmountDetails PaidAmount { get; set; }
    public AmountDetails Reconciliation { get; set; }
}

public class AmountDetails
{
    public string Amount { get; set; }
    public string Currency { get; set; }
}

public class FulfillmentDetails
{
    public string Status { get; set; }
    public ShipmentSummaryDetails ShipmentSummary { get; set; }
}

public class ShipmentSummaryDetails
{
    public string LineItemsSent { get; set; }
}

public class DeliveryDetails
{
    public AddressDetails Address { get; set; }
    public MethodDetails Method { get; set; }
    public PickupPointDetails PickupPoint { get; set; }
    public AmountDetails Cost { get; set; }
    public TimeDetails Time { get; set; }
    public bool Smart { get; set; }
    public int CalculatedNumberOfPackages { get; set; }
}

public class MethodDetails
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class PickupPointDetails
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public AddressDetails Address { get; set; }
}

public class TimeDetails
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public GuaranteedDetails Guaranteed { get; set; }
    public DispatchDetails Dispatch { get; set; }
}

public class GuaranteedDetails
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class DispatchDetails
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class InvoiceDetails
{
    public bool Required { get; set; }
    public AddressDetails Address { get; set; }
    public DateTime DueDate { get; set; }
}

public class LineItemDetails
{
    public string Id { get; set; }
    public OfferDetails Offer { get; set; }
    public int Quantity { get; set; }
    public AmountDetails OriginalPrice { get; set; }
    public AmountDetails Price { get; set; }
    public ReconciliationDetails Reconciliation { get; set; }
    public List<AdditionalServiceDetails> SelectedAdditionalServices { get; set; }
    public DateTime BoughtAt { get; set; }
}

public class OfferDetails
{
    public string Id { get; set; }
    public string Name { get; set; }
    public ExternalDetails External { get; set; }
}

public class ExternalDetails
{
    public string Id { get; set; }
}

public class ReconciliationDetails
{
    public AmountDetails Value { get; set; }
    public string Type { get; set; }
    public int Quantity { get; set; }
}

public class AdditionalServiceDetails
{
    public string DefinitionId { get; set; }
    public string Name { get; set; }
    public AmountDetails Price { get; set; }
    public int Quantity { get; set; }
}

public class SurchargeDetails
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Provider { get; set; }
    public DateTime FinishedAt { get; set; }
    public AmountDetails PaidAmount { get; set; }
    public AmountDetails Reconciliation { get; set; }
}

public class DiscountDetails
{
    public string Type { get; set; }
}

public class NoteDetails
{
    public string Text { get; set; }
}

public class MarketplaceDetails
{
    public string Id { get; set; }
}

public class SummaryDetails
{
    public AmountDetails TotalToPay { get; set; }
}