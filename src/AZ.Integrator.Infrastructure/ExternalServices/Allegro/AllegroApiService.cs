using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Web;
using AZ.Integrator.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using AZ.Integrator.Domain.Abstractions;

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
        var orderTypes = new[] { OrderTypes.ReadyForProcessing, OrderTypes.FulfillmentStatusChanged };
        var queryString = string.Join("&", orderTypes.Select(type => $"type={HttpUtility.UrlEncode(type)}"));
        
        using var response = await _httpClient.GetAsync($"order/events?{queryString}");
        
        response.EnsureSuccessStatusCode();

        var orderEvents = await response.Content.ReadFromJsonAsync<GetOrderEventsModel>();

        return orderEvents.Events;
    }

    public async Task<GetOrderDetailsModel> GetOrderDetails(Guid orderId)
    {
        using var response = await _httpClient.GetAsync($"order/checkout-forms/{orderId}");
        
        response.EnsureSuccessStatusCode();

        var orderDetails = await response.Content.ReadFromJsonAsync<GetOrderDetailsModel>();

        return orderDetails;
    }
}