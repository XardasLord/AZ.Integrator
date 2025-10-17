namespace AZ.Integrator.Catalog.Contracts.FurnitureModels;

public record FurnitureModelViewModel(
    string FurnitureCode,
    Guid TenantId,
    int Version,
    bool IsDeleted,
    DateTime? DeletedAt,
    Guid CreatedBy,
    DateTime CreatedAt,
    IReadOnlyCollection<PartDefinitionViewModel> PartDefinitions
);

public record PartDefinitionViewModel(
    Guid Id,
    string Name,
    DimensionsViewModel Dimensions,
    string Color,
    string AdditionalInfo
)
{
    public string FurnitureCode { get; init; }
    public Guid TenantId { get; init; }
};

public record DimensionsViewModel(
    int LengthMm,
    int WidthMm,
    int ThicknessMm
);
