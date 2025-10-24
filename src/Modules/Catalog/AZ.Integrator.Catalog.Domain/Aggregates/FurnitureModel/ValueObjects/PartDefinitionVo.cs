namespace AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;

public record PartDefinitionVo(
    int? PartDefinitionId,
    PartName Name,
    Dimensions Dimensions,
    Color Color,
    string? AdditionalInfo)
{
    internal PartDefinition ToPartDefinitionDomain()
    {
        return new PartDefinition(Name, Dimensions, Color, AdditionalInfo);
    }
}