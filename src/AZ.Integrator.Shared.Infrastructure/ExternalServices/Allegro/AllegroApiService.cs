using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Web;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro.RequestModels;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro;

public class AllegroApiService : IAllegroService
{
    private readonly HttpClient _httpClient;

    public AllegroApiService(IHttpClientFactory httpClientFactory, ICurrentUser currentUser)
    {
        _httpClient = httpClientFactory.CreateClient(ExternalHttpClientNames.AllegroHttpClientName);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", currentUser.AllegroAccessToken);
    }

    public async Task<IEnumerable<OrderEvent>> GetOrderEvents()
    {
        var orderTypes = new[] { AllegroOrderTypesEnum.ReadyForProcessing.Name };
        var queryString = string.Join("&", orderTypes.Select(type => $"type={HttpUtility.UrlEncode(type)}"));
        
        using var response = await _httpClient.GetAsync($"order/events?{queryString}");
        
        response.EnsureSuccessStatusCode();

        var orderEvents = await response.Content.ReadFromJsonAsync<GetOrderEventsModelResponse>();

        return orderEvents.Events;
    }

    public async Task<GetNewOrdersModelResponse> GetNewOrders(Shared.Application.ExternalServices.Allegro.Models.GetAllQueryFilters filters)
    {
        var queryParams = ApplyFilters(filters);

        using var response = await _httpClient.GetAsync($"order/checkout-forms?{queryParams}");
        
        response.EnsureSuccessStatusCode();

        var orders = await response.Content.ReadFromJsonAsync<GetNewOrdersModelResponse>();

        return orders;
    }

    private static string ApplyFilters(Shared.Application.ExternalServices.Allegro.Models.GetAllQueryFilters filters)
    {
        var queryParamsDictionary = new Dictionary<string, string>
        {
            { "limit", filters.Take.ToString() }, 
            { "offset", filters.Skip.ToString() }, 
            { "fulfillment.status", filters.OrderFulfillmentStatus }
        };  
        
        // TODO: Handle rest of parameters like skip, etc.

        return queryParamsDictionary.ToHttpQueryString();
    }

    public async Task<GetOrderDetailsModelResponse> GetOrderDetails(Guid orderId)
    {
        using var response = await _httpClient.GetAsync($"order/checkout-forms/{orderId}");
        
        response.EnsureSuccessStatusCode();

        var orderDetails = await response.Content.ReadFromJsonAsync<GetOrderDetailsModelResponse>();

        return orderDetails;
    }

    public async Task ChangeStatus(Guid orderNumber, AllegroFulfillmentStatusEnum allegroFulfillmentStatusEnum, string allegroAccessToken)
    {
        var payload = new ChangeStatusRequestPayload
        {
            Status = allegroFulfillmentStatusEnum.Name,
            ShipmentSummary = new ShipmentSummary
            {
                LineItemsSent = ShipmentSummaryLineItemsSentEnum.All.Name
            }
        };
        var payloadJson = System.Text.Json.JsonSerializer.Serialize(payload);
        var payloadContent = new StringContent(payloadJson, Encoding.UTF8, "application/vnd.allegro.public.v1+json");
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", allegroAccessToken);
        
        using var response = await _httpClient.PutAsync($"order/checkout-forms/{orderNumber}/fulfillment", payloadContent);

        response.EnsureSuccessStatusCode();
    }
}