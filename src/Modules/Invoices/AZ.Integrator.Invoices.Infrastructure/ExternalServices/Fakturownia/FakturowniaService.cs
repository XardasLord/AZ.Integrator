using System.Net.Http.Json;
using System.Text;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia.Models;
using AZ.Integrator.Invoices.Contracts.Dtos;
using AZ.Integrator.Invoices.Infrastructure.ExternalServices.Fakturownia.Models;
using AZ.Integrator.Shared.Infrastructure.ExternalServices;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using Microsoft.Extensions.Options;
using BuyerDto = AZ.Integrator.Invoices.Contracts.Dtos.BuyerDto;

namespace AZ.Integrator.Invoices.Infrastructure.ExternalServices.Fakturownia;

public class FakturowniaService(IHttpClientFactory httpClientFactory, IOptions<FakturowniaOptions> fakturowniaOptions)
    : IInvoiceService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(ExternalHttpClientNames.FakturowniaHttpClientName);
    private readonly FakturowniaOptions _options = fakturowniaOptions.Value;

    public async Task<CreateInvoiceResponse> GenerateInvoice(BuyerDto buyerDto, IReadOnlyList<InvoiceLineDto> invoiceItems, PaymentTermsDto paymentTermsDto, DeliveryDto deliveryDto)
    {
        var payload = new CreateInvoicePayload
        {
            ApiToken = _options.ApiKey,
            Invoice = new InvoiceData
            {
                Kind = "vat",
                Number = null,
                SellDate = paymentTermsDto.SellDate.ToString("yyyy-MM-dd"),
                IssueDate = paymentTermsDto.IssueDate.ToString("yyyy-MM-dd"),
                PaymentTo = paymentTermsDto.PaymentToDate.ToString("yyyy-MM-dd"),
                SellerName = "",
                SellerTaxNo = "",
                BuyerName = buyerDto.CompanyName ?? $"{buyerDto.FirstName} {buyerDto.LastName}",
                BuyerEmail = buyerDto.Email,
                BuyerTaxNo = buyerDto.TaxNo,
                BuyerStreet = buyerDto.Street,
                BuyerCity = buyerDto.City,
                BuyerPostCode = buyerDto.PostCode,
                BuyerOverride = true,
                Positions = [],
                Status = InvoiceDataStatusEnum.Paid.Name,
                PaymentType = paymentTermsDto.IsPaid ? InvoiceDataPaymentTypeEnum.Online.Name : InvoiceDataPaymentTypeEnum.Cod.Name
            }
        };
        
        AddInvoiceItems(invoiceItems, payload);
        AddDeliveryCost(deliveryDto, payload);
        
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

    private static void AddInvoiceItems(IReadOnlyList<InvoiceLineDto> invoiceLines, CreateInvoicePayload payload)
    {
        invoiceLines.ToList().ForEach(item =>
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

    private static void AddDeliveryCost(DeliveryDto deliveryDto, CreateInvoicePayload payload)
    {
        if (deliveryDto.Amount == 0)
            return;
        
        payload.Invoice.Positions.Add(GetDeliveryCostPosition(deliveryDto));
    }

    private static InvoicePosition GetDeliveryCostPosition(DeliveryDto deliveryDto) 
        => new()
        {
            Name = deliveryDto.DeliveryItemName,
            Quantity = deliveryDto.Quantity,
            Tax = 23,
            TotalPriceGross = deliveryDto.Amount * deliveryDto.Quantity
        };
}