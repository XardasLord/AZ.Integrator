namespace AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

public class OrderDetailsDto
{
    public Guid Id { get; set; }
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