using System.Net.Http.Json;
using System.Text;
using AZ.Integrator.Application.Common.ExternalServices.ShipX;
using AZ.Integrator.Application.Common.ExternalServices.ShipX.Models;
using AZ.Integrator.Domain.Abstractions;

namespace AZ.Integrator.Infrastructure.ExternalServices.ShipX;

public class ShipXApiService : IShipXService
{
    private readonly ICurrentUser _currentUser;
    private readonly HttpClient _httpClient;

    public ShipXApiService(IHttpClientFactory httpClientFactory, ICurrentUser currentUser)
    {
        _currentUser = currentUser;
        _httpClient = httpClientFactory.CreateClient(ExternalHttpClientNames.AllegroHttpClientName);
    }

    public async Task<ShipmentResponse> CreateShipment(Shipment shipment)
    {
        var shipmentContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(shipment), Encoding.UTF8, "application/json");
        
        using var response = await _httpClient.PostAsync($"v1/organizations/{_currentUser.ShipXOrganizationId}/shipments", shipmentContent);
        
        response.EnsureSuccessStatusCode();

        var shipmentResponse = await response.Content.ReadFromJsonAsync<ShipmentResponse>();

        return shipmentResponse;
    }
}