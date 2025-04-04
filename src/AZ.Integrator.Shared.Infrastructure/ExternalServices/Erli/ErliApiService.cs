﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Erli;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Erli;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.Erli.RequestModels;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Infrastructure.ErliAccount;
using Microsoft.EntityFrameworkCore;
using Order = AZ.Integrator.Shared.Application.ExternalServices.Erli.Order;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Erli;

public class ErliApiService(
    IHttpClientFactory httpClientFactory,
    ErliAccountDbContext dataViewContext, // TODO: Create DataViewContext for ErliAccounts
    ICurrentUser currentUser) : IErliService
{
    private static readonly JsonSerializerOptions JsonSerializerDefaultOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public async Task<GetOrdersModelResponse> GetOrders(GetAllQueryFilters filters, TenantId tenantId)
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

        var httpClient = await PrepareHttpClient(tenantId);
        using var response = await httpClient.PostAsync("orders/_search", payloadContent);

        response.EnsureSuccessStatusCode();

        var orders = await response.Content.ReadFromJsonAsync<List<Order>>();

        if (filters.OrderFulfillmentStatus.Any(status => status == AllegroFulfillmentStatusEnum.New.Name || status == AllegroFulfillmentStatusEnum.Processing.Name))
        {
            orders = orders
                .Where(x => x.SellerStatus == ErliOrderSellerStatusEnum.ReadyToProcess.Name)
                .ToList();
        }
        else if (filters.OrderFulfillmentStatus.Any(status => status == AllegroFulfillmentStatusEnum.ReadyForShipment.Name))
        {
            orders = orders
                .Where(x => x.DeliveryTracking?.Status is "readyToSend")
                .ToList();
            
        }
        else if (filters.OrderFulfillmentStatus.Any(status => status == AllegroFulfillmentStatusEnum.Sent.Name))
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

    public async Task<GetProductsModelResponse> GetProducts(GetProductTagsQueryFilters filters, TenantId tenantId)
    {
        var request = new GetProductsFiltersRequestPayload
        {
            Pagination = new Pagination
            {
                SortField = ProductPaginationHelper.ModifiedAtSortField,
                Order = ProductPaginationHelper.Desc,
                Limit = 200 // 200 is max
            },
            Fields = [ProductFieldsHelper.ExternalId, ProductFieldsHelper.Sku]
        };

        if (!string.IsNullOrWhiteSpace(filters.SearchText))
        {
            request.Filter = new Filter
            {
                Field = ProductFilterHelper.Sku,
                Operator = "=",
                Value = filters.SearchText
            };
        }
        else
        {
            request.Filter = new Filter
            {
                Field = ProductFilterHelper.Status,
                Operator = "=",
                Value = "active"
            };
        }
        
        var payloadContent = PrepareContentRequest(request);

        var httpClient = await PrepareHttpClient(tenantId);
        using var response = await httpClient.PostAsync("products/_search", payloadContent);

        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadFromJsonAsync<List<Product>>();

        var totalCount = products.Count;
        
        products = products
            .DistinctBy(x => x.Sku)
            .Skip(filters.Skip)
            .Take(filters.Take)
            .ToList();

        return new GetProductsModelResponse
        {
            Products = products,
            TotalCount = totalCount,
            Count = products.Count
        };
    }

    public async Task AssignTrackingNumber(string orderNumber, IEnumerable<string> trackingNumbers, 
        string vendor, string deliveryTrackingStatus, string tenantId)
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

            var httpClient = await PrepareHttpClient(tenantId);
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

    private async Task<HttpClient> PrepareHttpClient(TenantId tenantId)
    {
        var httpClient = httpClientFactory.CreateClient(ExternalHttpClientNames.ErliHttpClientName);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetApiKey(tenantId));

        return httpClient;
    }

    private async Task<string> GetApiKey(TenantId tenantId)
    {
        var tenantAccount = await dataViewContext.ErliAccounts.SingleOrDefaultAsync(x => x.TenantId == tenantId.Value);

        if (tenantAccount is null)
            throw new ApplicationException($"Tenant {tenantId.Value} does not exist");
        
        return tenantAccount.ApiKey;
    }
}