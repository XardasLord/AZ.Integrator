namespace AZ.Integrator.Stocks.Domain.Aggregates.ValueObjects;

public sealed record PackageCode(string Value)
{
    public static implicit operator string(PackageCode tenantId)
        => tenantId.Value;

    public static implicit operator PackageCode(string tenantId)
        => new(tenantId);

    public override string ToString() => Value;
}