using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Web;
using AZ.Integrator.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Infrastructure.ExternalServices.Allegro.RequestModels;
using AZ.Integrator.Infrastructure.UtilityExtensions;

namespace AZ.Integrator.Infrastructure.ExternalServices.Allegro;

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

    public async Task<GetNewOrdersModelResponse> GetNewOrders()
    {
        var queryParams = new Dictionary<string, string>()
        {
            { "limit", "100" }, 
            { "status", AllegroOrderTypesEnum.ReadyForProcessing.Name },
            { "fulfillment.status", AllegroFulfillmentStatusEnum.New.Name }
        }.ToHttpQueryString();

        using var response = await _httpClient.GetAsync($"order/checkout-forms?{queryParams}");
        
        response.EnsureSuccessStatusCode();

        var orders = await response.Content.ReadFromJsonAsync<GetNewOrdersModelResponse>();

        return orders;
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