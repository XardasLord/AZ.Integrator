namespace AZ.Integrator.Domain.Aggregates.DpdShipment.ValueObjects;

public sealed record Waybill(string Value)
{
    public static implicit operator string(Waybill number)
        => number.Value;

    public static implicit operator Waybill(string number)
        => new(number);

    public override string ToString() => Value;
}