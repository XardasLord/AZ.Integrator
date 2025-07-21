namespace AZ.Integrator.Stocks.Domain.Aggregates.StockGroup.ValueObjects;

public sealed record StockGroupName(string Value)
{
    public static implicit operator string(StockGroupName name)
        => name.Value;

    public static implicit operator StockGroupName(string name)
        => new(name);

    public override string ToString() => Value;
}