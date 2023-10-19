namespace AZ.Integrator.Domain.Aggregates.DpdShipment.ValueObjects;

public sealed record ParcelNumber(long Value)
{
    public static implicit operator long(ParcelNumber number)
        => number.Value;

    public static implicit operator ParcelNumber(long number)
        => new(number);

    public override string ToString() => Value.ToString();
}