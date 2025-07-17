namespace AZ.Integrator.Stocks.Domain.Aggregates.StockGroup.ValueObjects;

public sealed record StockGroupId(uint Value)
{
    public static implicit operator uint(StockGroupId groupId)
        => groupId.Value;

    public static implicit operator StockGroupId(uint groupId)
        => new(groupId);

    public override string ToString() => Value.ToString();
}