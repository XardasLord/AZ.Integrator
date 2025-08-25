using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Shopify;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Erli;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Shopify;
using AZ.Integrator.Shared.Application.ExternalServices.Shopify.GraphqlResponses;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Infrastructure.ShopifyAccount;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.EntityFrameworkCore;
using GetOrdersModelResponse = AZ.Integrator.Shared.Application.ExternalServices.Shopify.GetOrdersModelResponse;
using Order = AZ.Integrator.Shared.Application.ExternalServices.Shopify.GraphqlResponses.Order;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Shopify;

public class ShopifyApiService(ShopifyAccountDbContext shopifyAccountDbContext) : IShopifyService
{
    public async Task<GetOrdersModelResponse> GetOrders(GetAllQueryFilters filters, TenantId tenantId)
    {
        var graphqlClient = await PrepareGraphqlClient(tenantId);
        
        var ordersResponse = await FetchOrders(graphqlClient, filters);
        ValidateResponse(ordersResponse, "orders");
        
        var ordersCountResponse = await FetchOrdersCount(graphqlClient, filters);
        ValidateResponse(ordersCountResponse, "ordersCount");
        
        return new GetOrdersModelResponse
        {
            Orders = ordersResponse.Data.Orders.Edges.Select(e => e.Node),
            Count = ordersResponse.Data.Orders.Edges.Count,
            TotalCount = ordersCountResponse.Data.OrdersCount.Count
        };
    }

    public async Task AssignTrackingNumber(string orderNumber, IEnumerable<string> trackingNumbers, string vendor, string tenantId)
    {
        var graphqlClient = await PrepareGraphqlClient(tenantId);
        
        var orderResponse = await FetchOrder(graphqlClient, orderNumber);
        ValidateResponse(orderResponse, "orders");

        var fulfillmentOrderIds = orderResponse.Data.Orders.Edges
            .SelectMany(x => x.Node.FulfillmentOrders.Nodes.Select(fo => fo.Id))
            .ToList();

        trackingNumbers = trackingNumbers.ToList();
        
        foreach (var fulfillmentOrderId in fulfillmentOrderIds)
        {
            var createFulfillmentResponse = await CreateFulfillment(graphqlClient, fulfillmentOrderId, trackingNumbers.ToList(), vendor);
            ValidateResponse(createFulfillmentResponse, "createFulfillment");
        }
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
        var filter = BuildOrderFilterQuery(filters);

        // https://shopify.dev/docs/api/admin-graphql/latest/queries/orders
        var request = new GraphQLRequest
        {
            Query = """
                    query ($take: Int!, $sortKey: OrderSortKeys!, $reverse: Boolean!, $query: String) { 
                        orders(first: $take, sortKey: $sortKey, reverse: $reverse, query: $query) {
                            edges {
                                cursor
                                node {
                                    id
                                    createdAt
                                    name
                                    number
                                    fullyPaid
                                    note
                                    displayFinancialStatus
                                    displayFulfillmentStatus
                                    shippingLine {
                                        title
                                    }
                                    # Requires Shopify Plus plan
                                    # shippingAddress {
                                        # firstName
                                        # lastName
                                        # address1
                                        # address2
                                        # phone
                                        # city
                                        # countryCodeV2
                                    # }
                                    totalPriceSet {
                                        presentmentMoney { 
                                            amount
                                            currencyCode
                                        }
                                    }
                                    lineItems(first: 100) {
                                        nodes {
                                            id
                                            name
                                            quantity
                                            sku
                                            originalUnitPriceSet {
                                                presentmentMoney {
                                                    amount
                                                    currencyCode
                                                }
                                            }
                                        }
                                    }
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
            Variables = new
            {
                take = filters.Take,
                sortKey = "CREATED_AT",
                reverse = true,
                query = filter
            }
        };

        return await client.SendQueryAsync<GetOrdersResponse>(request);
    }

    private static async Task<GraphQLResponse<GetOrdersCountResponse>> FetchOrdersCount(GraphQLHttpClient client, GetAllQueryFilters filters)
    {
        var filter = BuildOrderFilterQuery(filters);
        
        var request = new GraphQLRequest
        {
            Query = """
                    query ($query: String) { 
                        ordersCount(query: $query) {
                            count
                            precision
                        }
                    }
                    """,
            Variables = new
            {
                query = filter
            }
        };

        return await client.SendQueryAsync<GetOrdersCountResponse>(request);
    }

    private static string BuildOrderFilterQuery(GetAllQueryFilters filters)
    {
        var filter = string.Empty;
        
        if (filters.OrderFulfillmentStatus.Any(status => status == AllegroFulfillmentStatusEnum.New.Name || status == AllegroFulfillmentStatusEnum.Processing.Name))
            filter = $"{ShopifyFulfillmentStatusGraphqlFilterEnum.New.Name} AND {ShopifyOrderStatusGraphqlFilterEnum.Open}";
        else if (filters.OrderFulfillmentStatus.Any(status => status == AllegroFulfillmentStatusEnum.ReadyForShipment.Name))
            filter = $"{ShopifyFulfillmentStatusGraphqlFilterEnum.ReadyToProcess.Name} AND {ShopifyOrderStatusGraphqlFilterEnum.Open}";
        else if (filters.OrderFulfillmentStatus.Any(status => status == AllegroFulfillmentStatusEnum.Sent.Name))
            filter = $"{ShopifyFulfillmentStatusGraphqlFilterEnum.Sent.Name} AND {ShopifyOrderStatusGraphqlFilterEnum.Closed}";
        
        return filter;
    }

    private static async Task<GraphQLResponse<GetOrdersResponse>> FetchOrder(GraphQLHttpClient client, string orderNumber)
    {
        var request = new GraphQLRequest
        {
            Query = """
                    query ($filter: String!) { 
                        orders(first: 1, query: $filter) {
                            edges {
                                node {
                                    id
                                    name
                                    fulfillmentOrders(first: 10, query: "status:open") {
                                        nodes {
                                            id
                                            status
                                            createdAt
                                        }
                                    }
                                }
                            }
                        }
                    }
                    """,
            Variables = new
            {
                filter = $"name:{orderNumber}"
            }
        };

        return await client.SendQueryAsync<GetOrdersResponse>(request);
    }

    private static async Task<GraphQLResponse<dynamic>> CreateFulfillment(GraphQLHttpClient client, string fulfillmentOrderId, List<string> trackingNumbers, string vendor)
    {
        // https://shopify.dev/docs/api/admin-graphql/latest/mutations/fulfillmentcreate
        var request = new GraphQLRequest
        {
            Query = """
                    mutation ($fulfillment: FulfillmentInput!, $message: String) { 
                        fulfillmentCreate(fulfillment: $fulfillment, message: $message) {
                            fulfillment {
                                id
                                status
                                createdAt
                                trackingInfo {
                                  number
                                  company
                                  url
                                }
                            }
                            userErrors {
                                field
                                message
                            }
                        }
                    }
                    """,
            Variables = new
            {
                fulfillment = new
                {
                    notifyCustomer = true,
                    trackingInfo = new
                    {
                        company = vendor,
                        numbers = trackingNumbers.ToArray()
                    },
                    lineItemsByFulfillmentOrder = new[] 
                    {
                        new
                        {
                            fulfillmentOrderId = fulfillmentOrderId
                        }
                    }
                }
            }
        };
        
        return await client.SendMutationAsync<dynamic>(request);
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