using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Web;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro.RequestModels;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro;

public class AllegroApiService : IAllegroService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AllegroAccountDataViewContext _dataViewContext;
    private readonly ICurrentUser _currentUser;

    public AllegroApiService(
        IHttpClientFactory httpClientFactory,
        AllegroAccountDataViewContext dataViewContext,
        ICurrentUser currentUser)
    {
        _httpClientFactory = httpClientFactory;
        _dataViewContext = dataViewContext;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<OrderEvent>> GetOrderEvents()
    {
        var orderTypes = new[] { AllegroOrderTypesEnum.ReadyForProcessing.Name };
        var queryString = string.Join("&", orderTypes.Select(type => $"type={HttpUtility.UrlEncode(type)}"));
        
        var httpClient = await PrepareHttpClient(_currentUser.TenantId);
        using var response = await httpClient.GetAsync($"order/events?{queryString}");
        
        response.EnsureSuccessStatusCode();

        var orderEvents = await response.Content.ReadFromJsonAsync<GetOrderEventsModelResponse>();

        return orderEvents.Events;
    }

    public async Task<GetNewOrdersModelResponse> GetOrders(GetAllQueryFilters filters)
    {
        var queryParams = ApplyFilters(filters);

        var httpClient = await PrepareHttpClient(_currentUser.TenantId);
        using var response = await httpClient.GetAsync($"order/checkout-forms?{queryParams}");
        
        response.EnsureSuccessStatusCode();

        var orders = await response.Content.ReadFromJsonAsync<GetNewOrdersModelResponse>();

        return orders;
    }

    public async Task<GetOrderDetailsModelResponse> GetOrderDetails(Guid orderId)
    {
        var httpClient = await PrepareHttpClient(_currentUser.TenantId);
        using var response = await httpClient.GetAsync($"order/checkout-forms/{orderId}");
        
        response.EnsureSuccessStatusCode();

        var orderDetails = await response.Content.ReadFromJsonAsync<GetOrderDetailsModelResponse>();

        return orderDetails;
    }

    public async Task<GetOrderProductTagsResponse> GetOfferTags(string offerId)
    {
        var httpClient = await PrepareHttpClient(_currentUser.TenantId);
        using var response = await httpClient.GetAsync($"sale/offers/{offerId}/tags");
        
        response.EnsureSuccessStatusCode();

        var productTags = await response.Content.ReadFromJsonAsync<GetOrderProductTagsResponse>();

        return productTags;
    }

    public async Task<GetOrderProductTagsResponse> GetRegisteredTags()
    {
        var httpClient = await PrepareHttpClient(_currentUser.TenantId);
        using var response = await httpClient.GetAsync($"sale/offer-tags");
        
        response.EnsureSuccessStatusCode();

        var tags = await response.Content.ReadFromJsonAsync<GetOrderProductTagsResponse>();

        return tags;
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

    public async Task AssignTrackingNumber(Guid orderNumber, string trackingNumber, string tenantId)
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
        var payloadContent = new StringContent(payloadJson, Encoding.UTF8, "application/vnd.allegro.public.v1+json");
        return payloadContent;
    }

    private async Task<HttpClient> PrepareHttpClient(TenantId tenantId)
    {
        var httpClient = _httpClientFactory.CreateClient(ExternalHttpClientNames.AllegroHttpClientName);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken(tenantId));

        return httpClient;
    } 

    private async Task<string> GetAccessToken(TenantId tenantId)
    {
        var tenantAccount = await _dataViewContext.AllegroAccounts.SingleAsync(x => x.TenantId == tenantId.Value);

        return tenantAccount.AccessToken;
    }
}