using System.Net.Http.Json;
using System.Text;
using AZ.Integrator.Application.Common.ExternalServices.ShipX;
using AZ.Integrator.Application.Common.ExternalServices.ShipX.Models;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.Aggregates.InpostShipment.ValueObjects;
using AZ.Integrator.Infrastructure.UtilityExtensions;

namespace AZ.Integrator.Infrastructure.ExternalServices.ShipX;

public class ShipXApiService : IShipXService
{
    private readonly ICurrentUser _currentUser;
    private readonly HttpClient _httpClient;

    public ShipXApiService(IHttpClientFactory httpClientFactory, ICurrentUser currentUser)
    {
        _currentUser = currentUser;
        _httpClient = httpClientFactory.CreateClient(ExternalHttpClientNames.ShipXHttpClientName);
    }

    public async Task<ShipmentResponse> CreateShipment(Shipment shipment)
    {
        var shipmentJson = System.Text.Json.JsonSerializer.Serialize(shipment);
        var shipmentContent = new StringContent(shipmentJson, Encoding.UTF8, "application/json");
        
        using var response = await _httpClient.PostAsync($"v1/organizations/{_currentUser.ShipXOrganizationId}/shipments", shipmentContent);
        
        response.EnsureSuccessStatusCode();

        var shipmentResponse = await response.Content.ReadFromJsonAsync<ShipmentResponse>();

        return shipmentResponse;
    }

    public async Task<byte[]> GenerateLabel(ShipmentNumber number)
    {
        var queryParams = new Dictionary<string, string>()
        {
            { "format", "Pdf" }, 
            { "type", "A6" }
        }.ToHttpQueryString();
        
        using var response = await _httpClient.GetAsync($"v1/shipments/{number.Value}/label?{queryParams}");
        
        response.EnsureSuccessStatusCode();

        await using var resultStream = await response.Content.ReadAsStreamAsync();

        var label = await resultStream.ReadAsByteArrayAsync();

        return label;
    }
}