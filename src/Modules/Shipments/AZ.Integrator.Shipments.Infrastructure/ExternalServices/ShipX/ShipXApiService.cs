using System.Net.Http.Json;
using System.Text;
using AZ.Integrator.Shared.Infrastructure.ExternalServices;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX.Models;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;
using Microsoft.Extensions.Options;

namespace AZ.Integrator.Shipments.Infrastructure.ExternalServices.ShipX;

public class ShipXApiService(IOptions<ShipXOptions> shipXOptions, IHttpClientFactory httpClientFactory)
    : IShipXService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(ExternalHttpClientNames.ShipXHttpClientName);

    public async Task<ShipmentResponse> CreateShipment(Shipment shipment)
    {
        var shipmentJson = System.Text.Json.JsonSerializer.Serialize(shipment);
        var shipmentContent = new StringContent(shipmentJson, Encoding.UTF8, "application/json");
        
        using var response = await _httpClient.PostAsync($"v1/organizations/{shipXOptions.Value.OrganizationId}/shipments", shipmentContent);
        
        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Console.WriteLine($"Błąd {response.StatusCode}: {responseBody}");

            throw new Exception($"Request failed: {response.StatusCode}, {responseBody}");
        }
        
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
        
        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Console.WriteLine($"Błąd {response.StatusCode}: {responseBody}");

            throw new Exception($"Request failed: {response.StatusCode}, {responseBody}");
        }
        
        response.EnsureSuccessStatusCode();

        await using var resultStream = await response.Content.ReadAsStreamAsync();

        var label = await resultStream.ReadAsByteArrayAsync();

        return label;
    }

    public async Task<byte[]> GenerateLabel(IEnumerable<ShipmentNumber> numbers)
    {
        var queryParams = new Dictionary<string, string>()
        {
            { "format", "Pdf" },
            { "type", "A6" }
        };
        
        numbers.ToList().ForEach(number => queryParams.Add("shipment_ids[]", number));

        var httpParams = queryParams.ToHttpQueryString();
        
        using var response = await _httpClient.GetAsync($"v1/organizations/{shipXOptions.Value.OrganizationId}/shipments/labels?{httpParams}");
        
        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Console.WriteLine($"Błąd {response.StatusCode}: {responseBody}");

            throw new Exception($"Request failed: {response.StatusCode}, {responseBody}");
        }
        
        response.EnsureSuccessStatusCode();

        await using var resultStream = await response.Content.ReadAsStreamAsync();

        var label = await resultStream.ReadAsByteArrayAsync();

        return label;
    }

    public async Task<ShipmentResponse> GetDetails(ShipmentNumber number)
    {
        using var response = await _httpClient.GetAsync($"v1/shipments/{number.Value}");
        
        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Console.WriteLine($"Błąd {response.StatusCode}: {responseBody}");

            throw new Exception($"Request failed: {response.StatusCode}, {responseBody}");
        }
        
        response.EnsureSuccessStatusCode();

        await using var resultStream = await response.Content.ReadAsStreamAsync();

        var shipmentResponse = await response.Content.ReadFromJsonAsync<ShipmentResponse>();

        return shipmentResponse;
    }
}