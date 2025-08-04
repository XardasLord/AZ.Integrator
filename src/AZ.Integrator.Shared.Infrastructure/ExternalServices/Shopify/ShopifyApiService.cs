using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Shopify;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Shopify.GraphqlResponses;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Infrastructure.ShopifyAccount;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.EntityFrameworkCore;
using GetOrdersModelResponse = AZ.Integrator.Shared.Application.ExternalServices.Shopify.GetOrdersModelResponse;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Shopify;

public class ShopifyApiService(ShopifyAccountDbContext shopifyAccountDbContext) : IShopifyService
{
    public async Task<GetOrdersModelResponse> GetOrders(GetAllQueryFilters filters, TenantId tenantId)
    {
        var graphqlClient = await PrepareGraphqlClient(tenantId);
        
        var ordersResponse = await FetchOrders(graphqlClient, filters);
        ValidateResponse(ordersResponse, "orders");
        
        var ordersCountResponse = await FetchOrdersCount(graphqlClient);
        ValidateResponse(ordersCountResponse, "ordersCount");
        
        return new GetOrdersModelResponse
        {
            Orders = ordersResponse.Data.Orders.Edges.Select(e => e.Node),
            Count = ordersResponse.Data.Orders.Edges.Count,
            TotalCount = ordersCountResponse.Data.OrdersCount.Count
        };
    }

    private async Task<GraphQLHttpClient> PrepareGraphqlClient(TenantId tenantId)
    {
        var account = await GetAccount(tenantId);
        
        var graphqlClient = new GraphQLHttpClient(
            account.ApiUrl,
            new SystemTextJsonSerializer());
        
        graphqlClient.HttpClient.BaseAddress = new Uri(account.ApiUrl);
        graphqlClient.HttpClient.DefaultRequestHeaders.Add("X-Shopify-Access-Token", account.AdminApiToken);

        return graphqlClient;
    }

    private async Task<ShopifyAccountViewModel> GetAccount(TenantId tenantId)
    {
        var tenantAccount = await shopifyAccountDbContext.ShopifyAccounts.SingleOrDefaultAsync(x => x.TenantId == tenantId.Value);

        if (tenantAccount is null)
            throw new ApplicationException($"Tenant '{tenantId.Value}' does not exist");
        
        return tenantAccount;
    }
    
    private static async Task<GraphQLResponse<GetOrdersResponse>> FetchOrders(GraphQLHttpClient client, GetAllQueryFilters filters)
    {
        var request = new GraphQLRequest
        {
            Query = """
                    query ($take: Int!) { 
                        orders(first: $take) {
                            edges {
                                cursor
                                node {
                                    id
                                    createdAt
                                    name
                                }
                            }
                            pageInfo {
                                hasNextPage
                                hasPreviousPage
                                startCursor
                                endCursor
                            }
                        }
                    }
                    """,
            Variables = new { filters.Take }
        };

        return await client.SendQueryAsync<GetOrdersResponse>(request);
    }

    private static async Task<GraphQLResponse<GetOrdersCountResponse>> FetchOrdersCount(GraphQLHttpClient client)
    {
        var request = new GraphQLRequest
        {
            Query = """
                    query { 
                        ordersCount {
                            count
                            precision
                        }
                    }
                    """
        };

        return await client.SendQueryAsync<GetOrdersCountResponse>(request);
    }

    private static void ValidateResponse<T>(GraphQLResponse<T> response, string context)
    {
        if (response.Errors is not null && response.Errors.Length != 0)
        {
            var errorMessages = string.Join(", ", response.Errors.Select(e => e.Message));
            throw new ApplicationException($"Error fetching {context} from Shopify: {errorMessages}");
        }
    }
}