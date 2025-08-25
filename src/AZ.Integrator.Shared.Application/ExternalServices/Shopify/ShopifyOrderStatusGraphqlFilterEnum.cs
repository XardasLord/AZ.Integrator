using Ardalis.SmartEnum;

namespace AZ.Integrator.Shared.Application.ExternalServices.Shopify;

public sealed class ShopifyOrderStatusGraphqlFilterEnum : SmartEnum<ShopifyOrderStatusGraphqlFilterEnum>
{
    // https://shopify.dev/docs/api/admin-graphql/latest/queries/orders#argument-query
    public static readonly ShopifyOrderStatusGraphqlFilterEnum Open = new("status:open", 0);
    public static readonly ShopifyOrderStatusGraphqlFilterEnum Closed = new("status:closed", 1);
    
    private ShopifyOrderStatusGraphqlFilterEnum(string name, int value) : base(name, value)
    {
    }
}
