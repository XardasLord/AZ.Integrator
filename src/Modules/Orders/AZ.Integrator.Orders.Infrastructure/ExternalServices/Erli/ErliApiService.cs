using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Integrations.Contracts;
using AZ.Integrator.Orders.Application.Common.ExternalServices.Erli;
using AZ.Integrator.Orders.Infrastructure.ExternalServices.Erli.RequestModels;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Erli;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
using AZ.Integrator.Shared.Infrastructure.ExternalServices;
using Order = AZ.Integrator.Shared.Application.ExternalServices.Erli.Order;

namespace AZ.Integrator.Orders.Infrastructure.ExternalServices.Erli;

public class ErliApiService(
    IHttpClientFactory httpClientFactory,
    IIntegrationsReadFacade integrationsReadFacade) : IErliService
{
    private static readonly JsonSerializerOptions JsonSerializerDefaultOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public async Task<GetOrdersModelResponse> GetOrders(GetAllQueryFilters filters, TenantId tenantId, SourceSystemId sourceSystemId)
    {
        var request = new GetOrdersFiltersRequestPayload
        {
            Pagination = new Pagination
            {
                SortField = OrderPaginationHelper.CreatedAtSortField,
                Order = OrderPaginationHelper.DescOrderField,
                Limit = 200 // 200 is max
            }
        };

        if (!string.IsNullOrWhiteSpace(filters.SearchText))
        {
            request.Filter = new Filter
            {
                Field = OrderFilterHelper.UserEmailField,
                Operator = "=",
                Value = filters.SearchText
            };
        }
        
        var payloadContent = PrepareContentRequest(request);

        var httpClient = await PrepareHttpClient(tenantId, sourceSystemId);
        using var response = await httpClient.PostAsync("orders/_search", payloadContent);

        response.EnsureSuccessStatusCode();

        var orders = await response.Content.ReadFromJsonAsync<List<Order>>();

        if (filters.OrderFulfillmentStatus == AllegroFulfillmentStatusEnum.Processing.Name)
        {
            orders = orders
                .Where(x => x.SellerStatus == ErliOrderSellerStatusEnum.ReadyToProcess.Name)
                .ToList();
        }
        else if (filters.OrderFulfillmentStatus == AllegroFulfillmentStatusEnum.ReadyForShipment.Name)
        {
            orders = orders
                .Where(x => x.DeliveryTracking?.Status is "readyToSend")
                .ToList();
            
        }
        else if (filters.OrderFulfillmentStatus == AllegroFulfillmentStatusEnum.Sent.Name)
        {
            orders = orders
                .Where(x => x.DeliveryTracking?.Status is "send" or "delivered")
                .ToList();
        }

        var totalCount = orders.Count;
        
        orders = orders
            .OrderByDescending(x => x.Created)
            .Skip(filters.Skip)
            .Take(filters.Take)
            .ToList();

        return new GetOrdersModelResponse
        {
            Orders = orders,
            TotalCount = totalCount,
            Count = orders.Count
        };
    }
    
    public async Task<Order> GetOrderDetails(string orderId, TenantId tenantId, SourceSystemId sourceSystemId)
    {
        var httpClient = await PrepareHttpClient(tenantId, sourceSystemId);
        using var response = await httpClient.GetAsync($"orders/{orderId}");

        response.EnsureSuccessStatusCode();

        var order = await response.Content.ReadFromJsonAsync<Order>();

        return order;
    }

    public async Task AssignTrackingNumber(string orderNumber, IEnumerable<string> trackingNumbers, 
        string vendor, string deliveryTrackingStatus, Guid tenantId, string sourceSystemId)
    {
        foreach (var trackingNumber in trackingNumbers)
        {
            var payload = new AssignTrackingNumberRequestPayload
            {
                DeliveryTracking = new DeliveryTrackingPayload
                {
                    Vendor = vendor,
                    Status = deliveryTrackingStatus,
                    TrackingNumber = trackingNumber
                },
                ExternalOrderId = orderNumber
            };
            var payloadContent = PrepareContentRequest(payload);

            var httpClient = await PrepareHttpClient(tenantId, sourceSystemId);
            using var response = await httpClient.PatchAsync($"orders/{orderNumber}", payloadContent);

            response.EnsureSuccessStatusCode();
        }
    }

    private static StringContent PrepareContentRequest(object payload)
    {
        var payloadJson = JsonSerializer.Serialize(payload, JsonSerializerDefaultOptions);
        var payloadContent = new StringContent(payloadJson, Encoding.UTF8, MediaTypeNames.Application.Json);
        
        return payloadContent;
    }

    private async Task<HttpClient> PrepareHttpClient(TenantId tenantId, SourceSystemId sourceSystemId)
    {
        var httpClient = httpClientFactory.CreateClient(ExternalHttpClientNames.ErliHttpClientName);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetApiKey(tenantId, sourceSystemId));

        return httpClient;
    }

    private async Task<string> GetApiKey(TenantId tenantId, SourceSystemId sourceSystemId)
    {
        var details = await integrationsReadFacade.GetActiveErliIntegrationDetails(tenantId, sourceSystemId)
            ?? throw new ApplicationException($"Erli integration for tenant '{tenantId.Value}' and SourceSystemID '{sourceSystemId}' does not exist");
        
        return details.ApiKey;
    }
}