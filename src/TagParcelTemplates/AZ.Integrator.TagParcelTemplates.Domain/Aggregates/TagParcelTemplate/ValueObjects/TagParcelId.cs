namespace AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.ValueObjects;

public sealed record TagParcelId(uint Value)
{
    public static implicit operator uint(TagParcelId tenantId)
        => tenantId.Value;

    public static implicit operator TagParcelId(uint tenantId)
        => new(tenantId);

    public override string ToString() => Value.ToString();
}