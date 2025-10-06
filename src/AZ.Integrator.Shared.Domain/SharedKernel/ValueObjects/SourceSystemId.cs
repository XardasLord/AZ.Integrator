namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public sealed record SourceSystemId(string Value)
{
    public static implicit operator string(SourceSystemId tenantId)
        => tenantId.Value;

    public static implicit operator SourceSystemId(string tenantId)
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