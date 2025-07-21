using System.Globalization;
using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Stocks.Domain.Aggregates.Stock.ValueObjects;

public sealed record ChangeQuantity
{
    public int Value { get; }
    
    private ChangeQuantity() { }

    public ChangeQuantity(int quantity)
    {
        Value = Guard.Against.ChangeQuantity(quantity, nameof(ChangeQuantity));
    }
    
    internal ChangeQuantity Revert() 
        => new(-Value);

    public static implicit operator double(ChangeQuantity quantity)
        => quantity.Value;
        
    public static implicit operator ChangeQuantity(int quantity)
        => new(quantity);
        
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}