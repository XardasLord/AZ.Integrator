namespace AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;

public sealed record FurnitureCode(string Value)
{
    public static implicit operator string(FurnitureCode code)
        => code.Value;

    public static implicit operator FurnitureCode(string code)
        => new(code);

    public override string ToString() => Value;
}
