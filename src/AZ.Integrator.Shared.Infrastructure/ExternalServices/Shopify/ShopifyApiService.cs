using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Shopify;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Infrastructure.ShopifyAccount;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using GetOrdersModelResponse = AZ.Integrator.Shared.Application.ExternalServices.Shopify.GetOrdersModelResponse;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Shopify;

public class ShopifyApiService(
    IHttpClientFactory httpClientFactory,
    ShopifyAccountDbContext shopifyAccountDbContext,
    ICurrentUser currentUser) : IShopifyService
{
    private static readonly JsonSerializerOptions JsonSerializerDefaultOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };
    
    public async Task<GetOrdersModelResponse> GetOrders(GetAllQueryFilters filters, TenantId tenantId)
    {
        // TODO: Integration with Shopify API to fetch orders via GraphQL.
        
        // var payloadContent = PrepareContentRequest(request);
        //
        // var httpClient = await PrepareHttpClient(tenantId);
        // using var response = await httpClient.PostAsync("orders/_search", payloadContent);
        //
        // response.EnsureSuccessStatusCode();
        
        // TODO: Implement the actual request to Shopify API to fetch orders.
        return await Task.FromResult(new GetOrdersModelResponse
        {
            Count = 0,
            TotalCount = 0
        });
    }

    private static StringContent PrepareContentRequest(object payload)
    {
        var payloadJson = JsonSerializer.Serialize(payload, JsonSerializerDefaultOptions);
        var payloadContent = new StringContent(payloadJson, Encoding.UTF8, MediaTypeNames.Application.Json);
        
        return payloadContent;
    }

    private async Task<HttpClient> PrepareHttpClient(TenantId tenantId)
    {
        var account = await GetAccount(tenantId);
        
        var httpClient = httpClientFactory.CreateClient(ExternalHttpClientNames.ErliHttpClientName);
        httpClient.BaseAddress = new Uri(account.ApiUrl);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("X-Shopify-Access-Token", account.AdminApiToken);

        return httpClient;
    }

    private async Task<ShopifyAccountViewModel> GetAccount(TenantId tenantId)
    {
        var tenantAccount = await shopifyAccountDbContext.ShopifyAccounts.SingleOrDefaultAsync(x => x.TenantId == tenantId.Value);

        if (tenantAccount is null)
            throw new ApplicationException($"Tenant {tenantId.Value} does not exist");
        
        return tenantAccount;
    }
}