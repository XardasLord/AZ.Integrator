namespace AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia.Models;

public class CreateInvoicePayload
{
    public string ApiToken { get; set; }
    public InvoiceData Invoice { get; set; }
    
}
public class InvoiceData
{
    public string Kind { get; set; }
    public string Number { get; set; }
    public DateTime SellDate { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime PaymentTo { get; set; }
    public string SellerName { get; set; }
    public string SellerTaxNo { get; set; }
    public string BuyerName { get; set; }
    public string BuyerEmail { get; set; }
    public string BuyerTaxNo { get; set; }
    public List<InvoicePosition> Positions { get; set; }
}

public class InvoicePosition
{
    public string Name { get; set; }
    public int Tax { get; set; }
    public decimal TotalPriceGross { get; set; }
    public int Quantity { get; set; }
}