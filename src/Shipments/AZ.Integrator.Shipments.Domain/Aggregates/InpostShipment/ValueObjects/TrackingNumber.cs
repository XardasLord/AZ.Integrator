using Ardalis.GuardClauses;
using AZ.Integrator.Shipments.Domain.Extensions;

namespace AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;

public sealed record TrackingNumber
{
    public string Value { get; }

    private TrackingNumber() { }

    public TrackingNumber(string number)
    {
        Value = Guard.Against.InpostTrackingNumber(number, nameof(TrackingNumber));
    }

    public static implicit operator string(TrackingNumber number)
        => number.Value;

    public static implicit operator TrackingNumber(string number)
        => new(number);

    public override string ToString() => Value;
}