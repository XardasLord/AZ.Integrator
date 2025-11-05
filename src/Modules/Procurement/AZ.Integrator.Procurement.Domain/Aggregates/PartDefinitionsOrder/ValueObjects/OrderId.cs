namespace AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;

public sealed record OrderId(uint Value)
{
    public static implicit operator uint(OrderId id)
        => id.Value;

    public static implicit operator OrderId(uint id)
        => new(id);

    public override string ToString() => Value.ToString();
}