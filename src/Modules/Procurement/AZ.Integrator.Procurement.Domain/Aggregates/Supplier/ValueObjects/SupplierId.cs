namespace AZ.Integrator.Procurement.Domain.Aggregates.Supplier.ValueObjects;

public sealed record SupplierId(uint Value)
{
    public static implicit operator uint(SupplierId tenantId)
        => tenantId.Value;

    public static implicit operator SupplierId(uint tenantId)
        => new(tenantId);

    public override string ToString() => Value.ToString();
}