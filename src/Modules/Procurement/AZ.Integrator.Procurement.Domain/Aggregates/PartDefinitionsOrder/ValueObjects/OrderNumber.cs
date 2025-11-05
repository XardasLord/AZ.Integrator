namespace AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;

public sealed record OrderNumber(string Value)
{
    public static implicit operator string(OrderNumber number)
        => number.Value;

    public static implicit operator OrderNumber(string number)
        => new(number);

    public override string ToString() => Value;
}