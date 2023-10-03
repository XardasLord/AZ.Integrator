using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Domain.Aggregates.InpostShipment.ValueObjects;

public sealed record AllegroOrderNumber
{
    public string Value { get; }

    private AllegroOrderNumber() { }

    public AllegroOrderNumber(string number)
    {
        Value = Guard.Against.AllegroOrderNumber(number, nameof(AllegroOrderNumber));
    }

    public static implicit operator string(AllegroOrderNumber number)
        => number.Value;

    public static implicit operator AllegroOrderNumber(string number)
        => new(number);

    public override string ToString() => Value;
}