using Ardalis.SmartEnum;

namespace AZ.Integrator.Shared.Application.ExternalServices.Shopify;

public sealed class ShopifyDeliveryTrackingVendorEnum : SmartEnum<ShopifyDeliveryTrackingVendorEnum>
{
    public static readonly ShopifyDeliveryTrackingVendorEnum InPost = new("Inpost", 0);
    // Also many others available from this documentation -> https://shopify.dev/docs/api/admin-graphql/latest/objects/FulfillmentTrackingInfo#supported-tracking-companies
    
    private ShopifyDeliveryTrackingVendorEnum(string name, int value) : base(name, value)
    {
    }
}
