using System.Globalization;
using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Stocks.Domain.Aggregates.Stock.ValueObjects;

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

    public static Quantity operator +(Quantity a, ChangeQuantity b)
    {
        var result = a.Value + b.Value;
        
        return new Quantity(result);
    }

    public static Quantity operator -(Quantity a, ChangeQuantity b)
    {
        var result = a.Value - b.Value;
        
        return new Quantity(result);
    }
}