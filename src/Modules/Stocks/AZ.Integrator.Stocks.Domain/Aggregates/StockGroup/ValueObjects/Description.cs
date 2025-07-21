namespace AZ.Integrator.Stocks.Domain.Aggregates.StockGroup.ValueObjects;

public sealed record Description(string Value)
{
    public static implicit operator string(Description name)
        => name.Value;

    public static implicit operator Description(string name)
        => new(name);

    public override string ToString() => Value;
}