namespace AZ.Integrator.Procurement.Domain.Aggregates.Supplier.ValueObjects;

public sealed record TelephoneNumber(string Value)
{
    public static implicit operator string(TelephoneNumber name)
        => name.Value;

    public static implicit operator TelephoneNumber(string name)
        => new(name);

    public static TelephoneNumber Create(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? new TelephoneNumber(string.Empty) : new TelephoneNumber(value.Trim());
    }

    public override string ToString() => Value;
}
