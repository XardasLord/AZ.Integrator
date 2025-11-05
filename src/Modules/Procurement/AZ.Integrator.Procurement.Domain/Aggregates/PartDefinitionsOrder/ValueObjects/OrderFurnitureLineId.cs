namespace AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;

public sealed record OrderFurnitureLineId(uint Value)
{
    public static implicit operator uint(OrderFurnitureLineId id)
        => id.Value;

    public static implicit operator OrderFurnitureLineId(uint id)
        => new(id);

    public override string ToString() => Value.ToString();
}