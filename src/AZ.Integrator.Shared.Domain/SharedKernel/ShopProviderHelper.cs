namespace AZ.Integrator.Domain.SharedKernel;

public static class ShopProviderHelper
{
    public static ShopProviderType GetShopProviderType(string sourceSystemId)
    {
        if (sourceSystemId.StartsWith("erli-")) 
            return ShopProviderType.Erli;
        
        if (sourceSystemId.StartsWith("allegro-"))
            return ShopProviderType.Allegro;
        
        if (sourceSystemId.StartsWith("shopify-")) 
            return ShopProviderType.Shopify;

        return ShopProviderType.Unknown;
    }
}