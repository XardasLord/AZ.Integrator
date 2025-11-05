namespace AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;

public sealed record OrderFurniturePartLineId(uint Value)
{
    public static implicit operator uint(OrderFurniturePartLineId id)
        => id.Value;

    public static implicit operator OrderFurniturePartLineId(uint id)
        => new(id);

    public override string ToString() => Value.ToString();
}