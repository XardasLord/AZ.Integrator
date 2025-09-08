using System.Net.Http.Json;
using System.Text;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia.Models;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using Microsoft.Extensions.Options;
using BuyerDetails = AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia.BuyerDetails;
using DeliveryDetails = AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia.DeliveryDetails;
using PaymentDetails = AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia.PaymentDetails;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Fakturownia;

public class FakturowniaService(IHttpClientFactory httpClientFactory, IOptions<FakturowniaOptions> fakturowniaOptions)
    : IInvoiceService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(ExternalHttpClientNames.FakturowniaHttpClientName);
    private readonly FakturowniaOptions _options = fakturowniaOptions.Value;

    public async Task<CreateInvoiceResponse> GenerateInvoice(BuyerDetails buyerDetails, List<InvoiceItem> invoiceItems, PaymentDetails paymentDetails, DeliveryDetails deliveryDetails)
    {
        var payload = new CreateInvoicePayload
        {
            ApiToken = _options.ApiKey,
            Invoice = new InvoiceData
            {
                Kind = "vat",
                Number = null,
                SellDate = paymentDetails.SellDate.ToString("yyyy-MM-dd"),
                IssueDate = paymentDetails.IssueDate.ToString("yyyy-MM-dd"),
                PaymentTo = paymentDetails.PaymentToDate.ToString("yyyy-MM-dd"),
                SellerName = "",
                SellerTaxNo = "",
                BuyerName = buyerDetails.CompanyName ?? $"{buyerDetails.FirstName} {buyerDetails.LastName}",
                BuyerEmail = buyerDetails.Email,
                BuyerTaxNo = buyerDetails.TaxNo,
                BuyerStreet = buyerDetails.Street,
                BuyerCity = buyerDetails.City,
                BuyerPostCode = buyerDetails.PostCode,
                BuyerOverride = true,
                Positions = [],
                Status = paymentDetails.IsPaid ? "paid" : "issued"
            }
        };
        
        AddInvoiceItems(invoiceItems, payload);
        AddDeliveryCost(deliveryDetails, payload);
        
        var invoiceJson = System.Text.Json.JsonSerializer.Serialize(payload);
        var invoiceContent = new StringContent(invoiceJson, Encoding.UTF8, "application/json");
        
        using var response = await _httpClient.PostAsync("invoices.json", invoiceContent);
        

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Console.WriteLine($"Błąd {response.StatusCode}: {responseBody}");

            throw new Exception($"Request failed: {response.StatusCode}, {responseBody}");
        }
        
        response.EnsureSuccessStatusCode();

        var invoiceResponse = await response.Content.ReadFromJsonAsync<CreateInvoiceResponse>();

        return invoiceResponse;
    }

    public async Task<byte[]> Download(long invoiceId)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "api_token", _options.ApiKey },
        }.ToHttpQueryString();
        
        using var response = await _httpClient.GetAsync($"invoices/{invoiceId}.pdf?{queryParams}");
        
        response.EnsureSuccessStatusCode();

        await using var resultStream = await response.Content.ReadAsStreamAsync();

        var label = await resultStream.ReadAsByteArrayAsync();

        return label;
    }

    private static void AddInvoiceItems(List<InvoiceItem> invoiceItems, CreateInvoicePayload payload)
    {
        invoiceItems.ForEach(item =>
        {
            payload.Invoice.Positions.Add(new InvoicePosition
            {
                Name = item.ItemName,
                Quantity = item.Quantity,
                Tax = 23,
                TotalPriceGross = item.Amount * item.Quantity
            });
        });
    }

    private static void AddDeliveryCost(DeliveryDetails deliveryDetails, CreateInvoicePayload payload)
    {
        if (deliveryDetails.Amount == 0)
            return;
        
        payload.Invoice.Positions.Add(GetDeliveryCostPosition(deliveryDetails));
    }

    private static InvoicePosition GetDeliveryCostPosition(DeliveryDetails deliveryDetails) 
        => new()
        {
            Name = deliveryDetails.DeliveryItemName,
            Quantity = deliveryDetails.Quantity,
            Tax = 23,
            TotalPriceGross = deliveryDetails.Amount * deliveryDetails.Quantity
        };
}