namespace AZ.Integrator.Catalog.Contracts.FurnitureModels;

public record CreateFurnitureModelRequest(
    string FurnitureCode,
    IEnumerable<AddPartDefinitionRequest> PartDefinitions
);

public record ImportFurnitureModelRequest(
    string FurnitureCode,
    IEnumerable<ImportPartDefinitionRequest> PartDefinitions
);

public record AddPartDefinitionRequest(
    string Name,
    int LengthMm,
    int WidthMm,
    int ThicknessMm,
    string Color,
    string AdditionalInfo
);

public record UpdateFurnitureModelRequest(
    string FurnitureCode,
    IEnumerable<UpdatePartDefinitionRequest> PartDefinitions
);

public record UpdatePartDefinitionRequest(
    int? Id,
    string Name,
    int LengthMm,
    int WidthMm,
    int ThicknessMm,
    string Color,
    string AdditionalInfo
);

public record ImportPartDefinitionRequest(
    string Name,
    int LengthMm,
    int WidthMm,
    int ThicknessMm,
    string Color,
    string AdditionalInfo
);
