using Ardalis.SmartEnum;

namespace AZ.Integrator.Shared.Application.ExternalServices.Shopify;

public sealed class ShopifyFulfillmentStatusGraphqlFilterEnum : SmartEnum<ShopifyFulfillmentStatusGraphqlFilterEnum>
{
    // https://shopify.dev/docs/api/admin-graphql/latest/queries/orders#argument-query
    public static readonly ShopifyFulfillmentStatusGraphqlFilterEnum New = new("fulfillment_status:unfulfilled", 0);
    public static readonly ShopifyFulfillmentStatusGraphqlFilterEnum ReadyToProcess = new("fulfillment_status:fulfilled", 1);
    public static readonly ShopifyFulfillmentStatusGraphqlFilterEnum Sent = new("fulfillment_status:shipped", 2);
    
    private ShopifyFulfillmentStatusGraphqlFilterEnum(string name, int value) : base(name, value)
    {
    }
}
