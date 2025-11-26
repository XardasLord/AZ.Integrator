using System.Net.Http.Json;
using System.Text;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Integrations.Contracts;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Invoices.Application.Common.Exceptions;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia.Models;
using AZ.Integrator.Invoices.Contracts.Dtos;
using AZ.Integrator.Invoices.Infrastructure.ExternalServices.Fakturownia.Models;
using AZ.Integrator.Shared.Infrastructure.ExternalServices;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using BuyerDto = AZ.Integrator.Invoices.Contracts.Dtos.BuyerDto;

namespace AZ.Integrator.Invoices.Infrastructure.ExternalServices.Fakturownia;

public class FakturowniaService(
    IHttpClientFactory httpClientFactory, 
    IIntegrationsReadFacade integrationsReadFacade) : IInvoiceService
{
    public async Task<CreateInvoiceResponse> GenerateInvoice(
        BuyerDto buyerDto,
        IReadOnlyList<InvoiceLineDto> invoiceItems,
        PaymentTermsDto paymentTermsDto,
        DeliveryDto deliveryDto,
        TenantId tenantId)
    {
        var integrationDetails = await integrationsReadFacade.GetActiveFakturowniaIntegrationDetails(tenantId)
            ?? throw new InvoiceGenerationException("No active fakturownia integration details found for tenant.");
        
        var payload = new CreateInvoicePayload
        {
            ApiToken =  integrationDetails.ApiKey,
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

        var httpClient = PrepareHttpClient(integrationDetails);
        
        using var response = await httpClient.PostAsync("invoices.json", invoiceContent);
        
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

    public async Task<byte[]> Download(long invoiceId, TenantId tenantId)
    {
        var integrationDetails = await integrationsReadFacade.GetActiveFakturowniaIntegrationDetails(tenantId)
                                 ?? throw new InvoiceGenerationException("No active fakturownia integration details found for tenant.");
        
        var queryParams = new Dictionary<string, string>
        {
            { "api_token", integrationDetails.ApiKey },
        }.ToHttpQueryString();
        
        var httpClient = PrepareHttpClient(integrationDetails);
        
        using var response = await httpClient.GetAsync($"invoices/{invoiceId}.pdf?{queryParams}");
        
        response.EnsureSuccessStatusCode();

        await using var resultStream = await response.Content.ReadAsStreamAsync();

        var label = await resultStream.ReadAsByteArrayAsync();

        return label;
    }

    private HttpClient PrepareHttpClient(FakturowniaIntegrationViewModel integrationDetails)
    {
        var httpClient = httpClientFactory.CreateClient(ExternalHttpClientNames.FakturowniaHttpClientName);
        
        httpClient.BaseAddress = new Uri(integrationDetails.ApiUrl);

        return httpClient;
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