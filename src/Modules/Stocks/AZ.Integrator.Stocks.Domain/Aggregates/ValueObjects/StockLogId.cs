namespace AZ.Integrator.Stocks.Domain.Aggregates.ValueObjects;

public sealed record StockLogId(uint Value)
{
    public static implicit operator uint(StockLogId tenantId)
        => tenantId.Value;

    public static implicit operator StockLogId(uint tenantId)
        => new(tenantId);

    public override string ToString() => Value.ToString();
}