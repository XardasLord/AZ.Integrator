namespace AZ.Integrator.Catalog.Contracts.FurnitureModels;

public record CreateFurnitureModelRequest(
    string FurnitureCode,
    IEnumerable<AddPartDefinitionRequest> PartDefinitions
);

public record UpdateFurnitureModelRequest(
    string FurnitureCode,
    IEnumerable<UpdatePartDefinitionRequest> PartDefinitions
);

public record AddPartDefinitionRequest(
    string Name,
    int LengthMm,
    int WidthMm,
    int ThicknessMm,
    int Quantity,
    string AdditionalInfo,
    EdgeBandingTypeViewModel LengthEdgeBandingType,
    EdgeBandingTypeViewModel WidthEdgeBandingType
);

public record UpdatePartDefinitionRequest(
    int? Id,
    string Name,
    int LengthMm,
    int WidthMm,
    int ThicknessMm,
    int Quantity,
    string AdditionalInfo,
    EdgeBandingTypeViewModel LengthEdgeBandingType,
    EdgeBandingTypeViewModel WidthEdgeBandingType
);
