﻿using System.Text.Json.Serialization;

namespace AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia.Models;

public class CreateInvoicePayload
{
    [JsonPropertyName("api_token")]
    public string ApiToken { get; set; }
    
    [JsonPropertyName("invoice")]
    public InvoiceData Invoice { get; set; }
    
}
public class InvoiceData
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; }
    
    [JsonPropertyName("number")]
    public string Number { get; set; }
    
    [JsonPropertyName("sell_date")]
    public string SellDate { get; set; }
    
    [JsonPropertyName("issue_date")]
    public string IssueDate { get; set; }
    
    [JsonPropertyName("payment_to")]
    public string PaymentTo { get; set; }
    
    [JsonPropertyName("seller_name")]
    public string SellerName { get; set; }
    
    [JsonPropertyName("seller_tax_no")]
    public string SellerTaxNo { get; set; }
    
    [JsonPropertyName("buyer_name")]
    public string BuyerName { get; set; }
    
    [JsonPropertyName("buyer_email")]
    public string BuyerEmail { get; set; }
    
    [JsonPropertyName("buyer_tax_no")]
    public string BuyerTaxNo { get; set; }
    
    [JsonPropertyName("positions")]
    public List<InvoicePosition> Positions { get; set; }
}

public class InvoicePosition
{
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("tax")]
    public int Tax { get; set; }
    
    [JsonPropertyName("total_price_gross")]
    public decimal TotalPriceGross { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}