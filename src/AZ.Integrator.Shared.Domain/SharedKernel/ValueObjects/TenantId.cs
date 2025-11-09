namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public sealed record TenantId(Guid Value)
{
    public static implicit operator Guid(TenantId tenantId)
        => tenantId.Value;

    public static implicit operator TenantId(Guid tenantId)
        => new(tenantId);

    public override string ToString() => Value.ToString();
}