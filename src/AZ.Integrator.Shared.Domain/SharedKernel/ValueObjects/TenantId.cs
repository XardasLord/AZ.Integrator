namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public sealed record TenantId(string Value)
{
    public static implicit operator string(TenantId tenantId)
        => tenantId.Value;

    public static implicit operator TenantId(string tenantId)
        => new(tenantId);

    public ShopProviderType GetShopProviderType() =>
        Value switch
        {
            _ when Value.StartsWith("allegro-") => ShopProviderType.Allegro,
            _ when Value.StartsWith("erli-")    => ShopProviderType.Erli,
            _ when Value.StartsWith("shopify-") => ShopProviderType.Shopify,
            _                                   => ShopProviderType.Unknown
        };

    public override string ToString() => Value;
}