namespace AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;

public record PartDefinitionVo(
    int? PartDefinitionId,
    PartName Name,
    Dimensions Dimensions,
    Quantity Quantity,
    string? AdditionalInfo)
{
    internal PartDefinition ToPartDefinitionDomain()
    {
        return new PartDefinition(Name, Dimensions, Quantity, AdditionalInfo);
    }
}