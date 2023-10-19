namespace AZ.Integrator.Domain.Aggregates.InpostShipment.ValueObjects;

public sealed record ShipmentNumber(string Value)
{
    public static implicit operator string(ShipmentNumber number)
        => number.Value;

    public static implicit operator ShipmentNumber(string number)
        => new(number);

    public override string ToString() => Value;
}