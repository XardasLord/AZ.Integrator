using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Web;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro.RequestModels;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.AllegroAccount;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro;

public class AllegroApiService(
    IHttpClientFactory httpClientFactory,
    AllegroAccountDataViewContext dataViewContext)
    : IAllegroService
{
    private const string AllegroMediaType = "application/vnd.allegro.public.v1+json";

    public async Task<IEnumerable<OrderEvent>> GetOrderEvents(string tenantId)
    {
        var orderTypes = new[] { AllegroOrderTypesEnum.ReadyForProcessing.Name };
        var queryString = string.Join("&", orderTypes.Select(type => $"type={HttpUtility.UrlEncode(type)}"));
        
        var httpClient = await PrepareHttpClient(tenantId);
        using var response = await httpClient.GetAsync($"order/events?{queryString}");
        
        response.EnsureSuccessStatusCode();

        var orderEvents = await response.Content.ReadFromJsonAsync<GetOrderEventsModelResponse>();

        return orderEvents.Events;
    }

    public async Task<GetNewOrdersModelResponse> GetOrders(GetAllQueryFilters filters, string tenantId)
    {
        var queryParams = ApplyFilters(filters);

        var httpClient = await PrepareHttpClient(tenantId);
        using var response = await httpClient.GetAsync($"order/checkout-forms?{queryParams}");
        
        response.EnsureSuccessStatusCode();

        var orders = await response.Content.ReadFromJsonAsync<GetNewOrdersModelResponse>();

        return orders;
    }

    public async Task<GetOrderDetailsModelResponse> GetOrderDetails(Guid orderId, string tenantId)
    {
        var httpClient = await PrepareHttpClient(tenantId);
        using var response = await httpClient.GetAsync($"order/checkout-forms/{orderId}");
        
        response.EnsureSuccessStatusCode();

        var orderDetails = await response.Content.ReadFromJsonAsync<GetOrderDetailsModelResponse>();

        return orderDetails;
    }

    public async Task<GetOffersResponse> GetOffers(GetProductTagsQueryFilters filters, string tenantId)
    {
        var queryParamsDictionary = new Dictionary<string, object>
        {
            { "limit", filters.Take.ToString() },
            { "offset", filters.Skip.ToString() }
        };

        if (!string.IsNullOrWhiteSpace(filters.SearchText))
        {
            queryParamsDictionary.Add("external.id", filters.SearchText);
        }

        var queryParams = queryParamsDictionary.ToHttpQueryString();
        
        var httpClient = await PrepareHttpClient(tenantId);
        using var response = await httpClient.GetAsync($"sale/offers?{queryParams}");
        
        response.EnsureSuccessStatusCode();

        var offers = await response.Content.ReadFromJsonAsync<GetOffersResponse>();

        return offers;
    }

    public async Task ChangeStatus(Guid orderNumber, AllegroFulfillmentStatusEnum allegroFulfillmentStatus, string tenantId)
    {
        var payload = new ChangeStatusRequestPayload
        {
            Status = allegroFulfillmentStatus.Name,
            ShipmentSummary = new ShipmentSummary
            {
                LineItemsSent = ShipmentSummaryLineItemsSentEnum.All.Name
            }
        };
        var payloadContent = PrepareContentRequest(payload);
        
        var httpClient = await PrepareHttpClient(tenantId);
        using var response = await httpClient.PutAsync($"order/checkout-forms/{orderNumber}/fulfillment", payloadContent);

        response.EnsureSuccessStatusCode();
    }

    public async Task AssignTrackingNumber(Guid orderNumber, IEnumerable<string> trackingNumbers, string tenantId)
    {
        foreach (var trackingNumber in trackingNumbers)
        {
            var payload = new AssignTrackingNumberRequestPayload
            {
                CarrierId = "INPOST",
                TrackingNumber = trackingNumber
            };
            var payloadContent = PrepareContentRequest(payload);

            var httpClient = await PrepareHttpClient(tenantId);
            using var response = await httpClient.PostAsync($"order/checkout-forms/{orderNumber}/shipments", payloadContent);

            response.EnsureSuccessStatusCode();
        }
    }

    private static string ApplyFilters(GetAllQueryFilters filters)
    {
        var queryParamsDictionary = new Dictionary<string, object>
        {
            { "limit", filters.Take.ToString() }, 
            { "offset", filters.Skip.ToString() }, 
            { "fulfillment.status", filters.OrderFulfillmentStatus }
        };

        if (filters.SearchText?.Length > 0)
        {
            queryParamsDictionary.Add("buyer.login", filters.SearchText);
        }

        return queryParamsDictionary.ToHttpQueryString();
    }

    private static StringContent PrepareContentRequest(object payload)
    {
        var payloadJson = System.Text.Json.JsonSerializer.Serialize(payload);
        var payloadContent = new StringContent(payloadJson, Encoding.UTF8, AllegroMediaType);
        return payloadContent;
    }

    private async Task<HttpClient> PrepareHttpClient(TenantId tenantId)
    {
        var httpClient = httpClientFactory.CreateClient(ExternalHttpClientNames.AllegroHttpClientName);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken(tenantId));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(AllegroMediaType));

        return httpClient;
    }

    private async Task<string> GetAccessToken(TenantId tenantId)
    {
        var tenantAccount = await dataViewContext.AllegroAccounts.SingleAsync(x => x.TenantId == tenantId.Value);

        return tenantAccount.AccessToken;
    }
}