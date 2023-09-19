using System.Net.Http.Headers;
using System.Net.Http.Json;
using AZ.Integrator.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Infrastructure.ExternalServices.Allegro.Models;

namespace AZ.Integrator.Infrastructure.ExternalServices.Allegro;

public class AllegroApiService : IAllegroService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICurrentUser _currentUser;

    public AllegroApiService(IHttpClientFactory httpClientFactory, ICurrentUser currentUser)
    {
        _httpClientFactory = httpClientFactory;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<OrderListDto>> GetOrdersReadyForProcessing()
    {
        var httpClient = _httpClientFactory.CreateClient("AllegroClient");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _currentUser.AllegroAccessToken);

        using var response = await httpClient.GetAsync("order/events");
        
        response.EnsureSuccessStatusCode();

        var orderEvents = await response.Content.ReadFromJsonAsync<GetOrderEventsModel>();

        return orderEvents.Events.Where(x => x.Type == OrderTypes.ReadyForProcessing).Select(x => new OrderListDto
        {
            OrderId = Guid.Parse(x.Order.CheckoutForm.Id)
        });;
    }
}