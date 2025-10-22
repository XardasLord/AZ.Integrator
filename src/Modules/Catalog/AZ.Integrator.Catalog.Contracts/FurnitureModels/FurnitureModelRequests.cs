namespace AZ.Integrator.Catalog.Contracts.FurnitureModels;

public record CreateFurnitureModelRequest(
    string FurnitureCode,
    IEnumerable<AddPartDefinitionRequest> PartDefinitions
);

public record AddPartDefinitionRequest(
    string Name,
    int LengthMm,
    int WidthMm,
    int ThicknessMm,
    string Color,
    string AdditionalInfo
);

public record UpdatePartDefinitionRequest(
    Guid PartDefinitionId,
    string Name,
    int LengthMm,
    int WidthMm,
    int ThicknessMm,
    string Color,
    string AdditionalInfo
);
