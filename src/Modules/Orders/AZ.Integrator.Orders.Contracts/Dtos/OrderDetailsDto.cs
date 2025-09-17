namespace AZ.Integrator.Orders.Contracts.Dtos;

public class OrderDetailsDto
{
    public string Id { get; set; }
    public string MessageToSeller { get; set; }
    public BuyerDetailsDto Buyer { get; set; }
    public PaymentDetailsDto Payment { get; set; }
    public string Status { get; set; }
    public FulfillmentDetailsDto Fulfillment { get; set; }
    public DeliveryDetailsDto Delivery { get; set; }
    public InvoiceDetailsDto Invoice { get; set; }
    public List<LineItemDetailsDto> LineItems { get; set; }
    public List<SurchargeDetailsDto> Surcharges { get; set; }
    public List<DiscountDetailsDto> Discounts { get; set; }
    public NoteDetailsDto Note { get; set; }
    
    [Obsolete("Available only in Allegro response. Can be removed?")]
    public MarketplaceDetailsDto Marketplace { get; set; }
    public SummaryDetailsDto Summary { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime PurchasedAt { get; set; }
    public string Revision { get; set; }
}

public class BuyerDetailsDto
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
    public PreferencesDetailsDto Preferences { get; set; }
    public AddressDetailsDto Address { get; set; }
}

public class PreferencesDetailsDto
{
    public string Language { get; set; }
}

public class AddressDetailsDto
{
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string CountryCode { get; set; }
    public InvoiceCompanyDetailsDto Company { get; set; }
    public InvoiceNaturalPersonDetailsDto NaturalPerson { get; set; }
}

public class InvoiceCompanyDetailsDto
{
    public string Name { get; set; }
    public string TaxId { get; set; }
}

public class InvoiceNaturalPersonDetailsDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class PaymentDetailsDto
{
    public string Id { get; set; }
    public string Type { get; set; }
    
    [Obsolete("Available only in Allegro response. Can be removed?")]
    public string Provider { get; set; }
    public DateTime? FinishedAt { get; set; }
    public AmountDetailsDto PaidAmount { get; set; }
    
    [Obsolete("Available only in Allegro response. Can be removed?")]
    public AmountDetailsDto Reconciliation { get; set; }
}

public class AmountDetailsDto
{
    public string Amount { get; set; }
    public string Currency { get; set; }
}

public class FulfillmentDetailsDto
{
    public string Status { get; set; }
    public ShipmentSummaryDetailsDto ShipmentSummary { get; set; }
}

public class ShipmentSummaryDetailsDto
{
    public string LineItemsSent { get; set; }
}

public class DeliveryDetailsDto
{
    public DeliveryAddressDetailsDto Address { get; set; }
    public MethodDetailsDto Method { get; set; }
    
    [Obsolete("Available only in Allegro response. Can be removed?")]
    public PickupPointDetailsDto PickupPoint { get; set; }
    public AmountDetailsDto Cost { get; set; }
    
    [Obsolete("Available only in Allegro response. Can be removed?")]
    public TimeDetailsDto Time { get; set; }

    [Obsolete("Available only in Allegro response. Can be removed?")]
    public bool Smart { get; set; }

    [Obsolete("Available only in Allegro response. Can be removed?")]
    public int? CalculatedNumberOfPackages { get; set; }
    public bool Cod { get; set; }
}

public class DeliveryAddressDetailsDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string CountryCode { get; set; }
    public string CompanyName { get; set; }
    public string PhoneNumber { get; set; }
}

public class MethodDetailsDto
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class PickupPointDetailsDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public AddressDetailsDto Address { get; set; }
}

public class TimeDetailsDto
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public GuaranteedDetailsDto Guaranteed { get; set; }
    public DispatchDetailsDto Dispatch { get; set; }
}

public class GuaranteedDetailsDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class DispatchDetailsDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class InvoiceDetailsDto
{
    public bool Required { get; set; }
    public AddressDetailsDto Address { get; set; }
    public string DueDate { get; set; }
}

public class LineItemDetailsDto
{
    public string Id { get; set; }
    public OfferDetailsDto Offer { get; set; }
    public int Quantity { get; set; }
    public AmountDetailsDto OriginalPrice { get; set; }
    public AmountDetailsDto Price { get; set; }
    // public ReconciliationDetailsDto Reconciliation { get; set; }
    // public List<AdditionalServiceDetailsDto> SelectedAdditionalServices { get; set; }
    public DateTime BoughtAt { get; set; }
}

public class OfferDetailsDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public ExternalDetailsDto External { get; set; }
}

public class ExternalDetailsDto
{
    public string Id { get; set; }
}

// public class ReconciliationDetailsDto
// {
//     public AmountDetailsDto Value { get; set; }
//     public string Type { get; set; }
//     public int Quantity { get; set; }
// }

// public class AdditionalServiceDetailsDto
// {
//     public string DefinitionId { get; set; }
//     public string Name { get; set; }
//     public AmountDetailsDto Price { get; set; }
//     public int Quantity { get; set; }
// }

public class SurchargeDetailsDto
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Provider { get; set; }
    public DateTime FinishedAt { get; set; }
    public AmountDetailsDto PaidAmount { get; set; }
    public AmountDetailsDto Reconciliation { get; set; }
}

public class DiscountDetailsDto
{
    public string Type { get; set; }
}

public class NoteDetailsDto
{
    public string Text { get; set; }
}

[Obsolete("Available only in Allegro response. Can be removed?")]
public class MarketplaceDetailsDto
{
    public string Id { get; set; }
}

public class SummaryDetailsDto
{
    public AmountDetailsDto TotalToPay { get; set; }
}