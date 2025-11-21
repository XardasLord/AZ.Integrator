using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Contracts;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Shared.Infrastructure.ExternalServices;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using AZ.Integrator.Shipments.Application.Common.Exceptions;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX.Models;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Infrastructure.ExternalServices.ShipX;

public class ShipXApiService(
    IHttpClientFactory httpClientFactory,
    IIntegrationsReadFacade integrationsReadFacade,
    ICurrentUser currentUser)
    : IShipXService
{
    public async Task<ShipmentResponse> CreateShipment(Shipment shipment)
    {
        var shipmentJson = System.Text.Json.JsonSerializer.Serialize(shipment);
        var shipmentContent = new StringContent(shipmentJson, Encoding.UTF8, "application/json");
        
        var integrationDetails = await integrationsReadFacade.GetInpostIntegrationDetails(currentUser.TenantId) 
                                 ?? throw new InpostShipmentCreationException($"No active Inpost ShipX integration details found for tenant '{currentUser.TenantId}'.");
        
        var httpClient = PrepareHttpClient(integrationDetails);
        
        using var response = await httpClient.PostAsync($"v1/organizations/{integrationDetails.OrganizationId}/shipments", shipmentContent);
        
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
        
        var integrationDetails = await integrationsReadFacade.GetInpostIntegrationDetails(currentUser.TenantId)
                                 ?? throw new InpostShipmentCreationException($"No active Inpost ShipX integration details found for tenant '{currentUser.TenantId}'.");
        var httpClient = PrepareHttpClient(integrationDetails);
        
        using var response = await httpClient.GetAsync($"v1/shipments/{number.Value}/label?{queryParams}");
        
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
        
        var integrationDetails = await integrationsReadFacade.GetInpostIntegrationDetails(currentUser.TenantId)
                                 ?? throw new InpostShipmentCreationException($"No active Inpost ShipX integration details found for tenant '{currentUser.TenantId}'.");
        var httpClient = PrepareHttpClient(integrationDetails);
        
        using var response = await httpClient.GetAsync($"v1/organizations/{integrationDetails.OrganizationId}/shipments/labels?{httpParams}");
        
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
        var integrationDetails = await integrationsReadFacade.GetInpostIntegrationDetails(currentUser.TenantId)
                                 ?? throw new InpostShipmentCreationException($"No active Inpost ShipX integration details found for tenant '{currentUser.TenantId}'.");
        var httpClient = PrepareHttpClient(integrationDetails);
        
        using var response = await httpClient.GetAsync($"v1/shipments/{number.Value}");
        
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

    private HttpClient PrepareHttpClient(InpostIntegrationViewModel integrationDetails)
    {
        var httpClient = httpClientFactory.CreateClient(ExternalHttpClientNames.ShipXHttpClientName);
        
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", integrationDetails.AccessToken);

        return httpClient;
    }
}