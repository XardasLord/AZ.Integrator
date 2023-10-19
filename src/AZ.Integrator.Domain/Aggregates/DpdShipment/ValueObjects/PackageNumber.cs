namespace AZ.Integrator.Domain.Aggregates.DpdShipment.ValueObjects;

public sealed record PackageNumber(long Value)
{
    public static implicit operator long(PackageNumber number)
        => number.Value;

    public static implicit operator PackageNumber(long number)
        => new(number);

    public override string ToString() => Value.ToString();
}