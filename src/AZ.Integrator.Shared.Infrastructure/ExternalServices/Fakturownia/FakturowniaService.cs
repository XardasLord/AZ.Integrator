using System.Globalization;
using System.Net.Http.Json;
using System.Text;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using Microsoft.Extensions.Options;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Fakturownia;

public class FakturowniaService : IInvoiceService
{
    private readonly HttpClient _httpClient;
    private readonly FakturowniaOptions _options;

    public FakturowniaService(IHttpClientFactory httpClientFactory, IOptions<FakturowniaOptions> fakturowniaOptions)
    {
        _httpClient = httpClientFactory.CreateClient(ExternalHttpClientNames.FakturowniaHttpClientName);
        _options = fakturowniaOptions.Value;
    }
    
    public async Task<CreateInvoiceResponse> GenerateInvoice(BuyerDetails buyerDetails, List<LineItemDetails> lineItems, PaymentDetails paymentDetails, DeliveryDetails deliveryDetails)
    {
        var payload = new CreateInvoicePayload
        {
            ApiToken = _options.ApiKey,
            Invoice = new InvoiceData
            {
                Kind = "vat",
                Number = null,
                SellDate = paymentDetails.FinishedAt?.ToString("yyyy-MM-dd") ?? DateTime.UtcNow.ToString("yyyy-MM-dd"),
                IssueDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                PaymentTo = paymentDetails.FinishedAt?.ToString("yyyy-MM-dd") ?? DateTime.UtcNow.ToString("yyyy-MM-dd"),
                SellerName = "",
                SellerTaxNo = "",
                BuyerName = buyerDetails.CompanyName?.ToString() ?? $"{buyerDetails.FirstName} {buyerDetails.LastName}",
                BuyerEmail = buyerDetails.Email,
                BuyerTaxNo = buyerDetails.PersonalIdentity,
                Positions = new List<InvoicePosition>()
            }
        };
        
        AddInvoiceItems(lineItems, payload);
        AddDeliveryCost(deliveryDetails, payload);
        
        var invoiceJson = System.Text.Json.JsonSerializer.Serialize(payload);
        var invoiceContent = new StringContent(invoiceJson, Encoding.UTF8, "application/json");
        
        using var response = await _httpClient.PostAsync("invoices.json", invoiceContent);
        
        response.EnsureSuccessStatusCode();

        var invoiceResponse = await response.Content.ReadFromJsonAsync<CreateInvoiceResponse>();

        return invoiceResponse;
    }

    private static void AddInvoiceItems(List<LineItemDetails> lineItems, CreateInvoicePayload payload)
    {
        lineItems.ForEach(item =>
        {
            payload.Invoice.Positions.Add(new InvoicePosition
            {
                Name = item.Offer.Name,
                Quantity = item.Quantity,
                Tax = 23,
                TotalPriceGross = decimal.Parse(item.Price.Amount, CultureInfo.InvariantCulture) * item.Quantity
            });
        });
    }

    private static void AddDeliveryCost(DeliveryDetails deliveryDetails, CreateInvoicePayload payload)
    {
        if (deliveryDetails.Cost is null || decimal.Parse(deliveryDetails.Cost.Amount, CultureInfo.InvariantCulture) == 0)
            return;
        
        payload.Invoice.Positions.Add(GetDeliveryCostPosition(deliveryDetails));
    }

    private static InvoicePosition GetDeliveryCostPosition(DeliveryDetails deliveryDetails) 
        => new()
        {
            Name = "KURIER",
            Quantity = 1,
            Tax = 23,
            TotalPriceGross = decimal.Parse(deliveryDetails.Cost.Amount, CultureInfo.InvariantCulture)
        };
}