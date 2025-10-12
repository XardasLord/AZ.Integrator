namespace AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;

public sealed record Color(string Value)
{
    public static implicit operator string(Color color)
        => color.Value;

    public static implicit operator Color(string color)
        => new(color);

    public static Color Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        return new Color(value.Trim());
    }

    public override string ToString() => Value;
}
