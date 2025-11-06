namespace AZ.Integrator.Procurement.Contracts.PartDefinitionOrders;

public record CreateOrderRequest(
    uint SupplierId,
    IEnumerable<CreateFurnitureLineRequest> FurnitureLineRequests
);

public record CreateFurnitureLineRequest(
    string FurnitureCode,
    int FurnitureVersion,
    int QuantityOrdered,
    IEnumerable<CreateFurniturePartLineRequest> PartDefinitionLines
);

public record CreateFurniturePartLineRequest(
    string PartName,
    int LengthMm,
    int WidthMm,
    int ThicknessMm,
    int Quantity,
    string AdditionalInfo,
    OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel LengthOrderFurniturePartLineDimensionsEdgeBandingType,
    OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel WidthOrderFurniturePartLineDimensionsEdgeBandingType
);
