namespace AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;

public sealed record PartName(string Value)
{
    public static implicit operator string(PartName name)
        => name.Value;

    public static implicit operator PartName(string name)
        => new(name);

    public static PartName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Part name cannot be null or empty", nameof(value));

        return new PartName(value.Trim());
    }

    public override string ToString() => Value;
}
