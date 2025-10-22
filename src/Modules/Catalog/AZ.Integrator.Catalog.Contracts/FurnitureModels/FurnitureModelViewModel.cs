namespace AZ.Integrator.Catalog.Contracts.FurnitureModels;

public class FurnitureModelViewModel
{
    public string FurnitureCode { get; init; }
    public Guid TenantId { get; init; }
    public int Version { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
    public IReadOnlyCollection<PartDefinitionViewModel> PartDefinitions { get; init; }
};

public class PartDefinitionViewModel
{
    public int Id { get; init; }
    public string FurnitureCode { get; init; }
    public Guid TenantId { get; init; }
    public string Name { get; init; }
    public DimensionsViewModel Dimensions { get; init; }
    public string Color { get; init; }
    public string? AdditionalInfo { get; init; }
};

public class DimensionsViewModel
{
    public int LengthMm { get; init; }
    public int WidthMm { get; init; }
    public int ThicknessMm { get; init; }
};
