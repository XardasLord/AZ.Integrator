namespace AZ.Integrator.Stocks.Domain.Aggregates.Stock.ValueObjects;

public sealed record PackageCode(string Value)
{
    public static implicit operator string(PackageCode code)
        => code.Value;

    public static implicit operator PackageCode(string code)
        => new(code);

    public override string ToString() => Value;
}