using System.Globalization;
using AZ.Integrator.Domain.SharedKernel.Exceptions;

namespace AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;

public sealed record Quantity
{
    public int Value { get; }
    
    private Quantity() { }

    public Quantity(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidQuantityException(quantity, "Quantity must be greater than zero.");
        
        Value = quantity;
    }

    public static implicit operator double(Quantity quantity)
        => quantity.Value;
        
    public static implicit operator Quantity(int quantity)
        => new(quantity);
        
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}