namespace AZ.Integrator.Domain.SharedKernel;

public static class TenantHelper
{
    public static ShopProviderType GetShopProviderType(string tenantId)
    {
        if (tenantId.StartsWith("erli-")) 
            return ShopProviderType.Erli;
        
        if (tenantId.StartsWith("allegro-"))
            return ShopProviderType.Allegro;

        return ShopProviderType.Unknown;
    }
}