namespace AZ.Integrator.Procurement.Application.Common.Email.Models;

public class PartDefinitionsOrderEmailModel
{
    public required string OrderNumber { get; init; }
    public required DateTime OrderDate { get; init; }
    public required string SupplierName { get; init; }
    public string? AdditionalNotes { get; init; }
    public required IReadOnlyCollection<FurnitureLineEmailModel> FurnitureLines { get; init; }
}

public class FurnitureLineEmailModel
{
    public required string FurnitureCode { get; init; }
    public int FurnitureVersion { get; init; }
    public int QuantityOrdered { get; init; }
    public required IReadOnlyCollection<PartLineEmailModel> PartLines { get; init; }
}

public class PartLineEmailModel
{
    public required string Name { get; init; }
    public int LengthMm { get; init; }
    public int WidthMm { get; init; }
    public int ThicknessMm { get; init; }
    public required string LengthEdgeBanding { get; init; }
    public required string WidthEdgeBanding { get; init; }
    public int Quantity { get; init; }
    public int TotalQuantity { get; init; }
    public string? AdditionalInfo { get; init; }
}

