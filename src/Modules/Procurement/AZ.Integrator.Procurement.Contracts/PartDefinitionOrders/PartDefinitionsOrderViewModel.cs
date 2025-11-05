namespace AZ.Integrator.Procurement.Contracts.PartDefinitionOrders;

public class PartDefinitionsOrderViewModel
{
    public int Id { get; init; }
    public required string Number { get; init; }
    public Guid TenantId { get; init; }
    public int SupplierId { get; init; }
    public OrderStatusViewModel Status { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid ModifiedBy { get; init; }
    public DateTime ModifiedAt { get; init; }
    public required IReadOnlyCollection<OrderFurnitureLineViewModel> FurnitureLines { get; init; }
}

public class OrderFurnitureLineViewModel
{
    public int Id { get; init; }
    public int OrderId { get; init; }
    public Guid TenantId { get; init; }
    public required string FurnitureCode { get; init; }
    public int FurnitureVersion { get; init; }
    public int QuantityOrdered { get; init; }
    public required IReadOnlyCollection<OrderFurniturePartLineViewModel> PartLines { get; init; }
}

public class OrderFurniturePartLineViewModel
{
    public int Id { get; init; }
    public int OrderFurnitureLineId { get; init; }
    public required string Name { get; init; }
    public DimensionsViewModel Dimensions { get; init; }
    public int Quantity { get; init; }
    public string? AdditionalInfo { get; init; }
}

public class DimensionsViewModel
{
    public int LengthMm { get; init; }
    public int WidthMm { get; init; }
    public int ThicknessMm { get; init; }
    public EdgeBandingTypeViewModel LengthEdgeBandingType { get; init; }
    public EdgeBandingTypeViewModel WidthEdgeBandingType { get; init; }
}

public enum OrderStatusViewModel
{
    Registered = 10,
    Sent = 20
}

public enum EdgeBandingTypeViewModel
{
    None = 0,
    One = 1,
    Two = 2
}
