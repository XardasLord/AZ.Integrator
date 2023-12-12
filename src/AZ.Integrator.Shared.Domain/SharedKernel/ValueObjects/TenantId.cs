namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public sealed record TenantId(string Value)
{
    public static implicit operator string(TenantId tenantId)
        => tenantId.Value;

    public static implicit operator TenantId(string tenantId)
        => new(tenantId);

    public override string ToString() => Value;
}