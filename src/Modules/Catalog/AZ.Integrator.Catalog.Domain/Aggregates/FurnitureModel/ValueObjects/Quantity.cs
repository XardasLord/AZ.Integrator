using System.Globalization;
using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;

public sealed record Quantity
{
    public int Value { get; }
    
    private Quantity() { }

    public Quantity(int quantity)
    {
        Value = Guard.Against.Quantity(quantity, nameof(Quantity));
    }

    public static implicit operator double(Quantity quantity)
        => quantity.Value;
        
    public static implicit operator Quantity(int quantity)
        => new(quantity);
        
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}