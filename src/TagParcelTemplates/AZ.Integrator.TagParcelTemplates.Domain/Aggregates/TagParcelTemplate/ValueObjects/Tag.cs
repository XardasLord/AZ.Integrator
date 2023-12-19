namespace AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.ValueObjects;

public sealed record Tag(string Value)
{
    public static implicit operator string(Tag tenantId)
        => tenantId.Value;

    public static implicit operator Tag(string tenantId)
        => new(tenantId);

    public override string ToString() => Value;
}