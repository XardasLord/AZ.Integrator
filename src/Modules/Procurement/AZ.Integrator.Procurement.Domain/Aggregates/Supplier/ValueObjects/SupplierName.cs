namespace AZ.Integrator.Procurement.Domain.Aggregates.Supplier.ValueObjects;

public sealed record SupplierName(string Value)
{
    public static implicit operator string(SupplierName name)
        => name.Value;

    public static implicit operator SupplierName(string name)
        => new(name);

    public static SupplierName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Supplier name cannot be null or empty", nameof(value));

        return new SupplierName(value.Trim());
    }

    public override string ToString() => Value;
}
