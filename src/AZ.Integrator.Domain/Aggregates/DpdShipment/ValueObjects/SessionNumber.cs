namespace AZ.Integrator.Domain.Aggregates.DpdShipment.ValueObjects;

public sealed record SessionNumber(long Value)
{
    public static implicit operator long(SessionNumber number)
        => number.Value;

    public static implicit operator SessionNumber(long number)
        => new(number);

    public override string ToString() => Value.ToString();
}