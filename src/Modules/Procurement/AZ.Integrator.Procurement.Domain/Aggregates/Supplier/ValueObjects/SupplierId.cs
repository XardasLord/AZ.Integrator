namespace AZ.Integrator.Procurement.Domain.Aggregates.Supplier.ValueObjects;

public sealed record SupplierId(uint Value)
{
    public static implicit operator uint(SupplierId id)
        => id.Value;

    public static implicit operator SupplierId(uint id)
        => new(id);

    public override string ToString() => Value.ToString();
}