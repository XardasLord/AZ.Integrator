using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Domain.Aggregates.InpostShipment.ValueObjects;

public sealed record ShipmentNumber
{
    public string Value { get; }

    private ShipmentNumber() { }

    public ShipmentNumber(string number)
    {
        Value = Guard.Against.ShipmentNumber(number, nameof(ShipmentNumber));
    }

    public static implicit operator string(ShipmentNumber number)
        => number.Value;

    public static implicit operator ShipmentNumber(string number)
        => new(number);

    public override string ToString() => Value;
}