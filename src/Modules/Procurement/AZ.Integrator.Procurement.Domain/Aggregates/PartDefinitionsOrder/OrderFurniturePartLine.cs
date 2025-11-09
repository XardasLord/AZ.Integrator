using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;

namespace AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder;

public class OrderFurniturePartLine : Entity<OrderFurniturePartLineId>
{
    private PartName _partName = null!;
    private Dimensions _dimensions = null!;
    private Quantity _quantity = null!;
    private string? _additionalInfo;
    
    public PartName PartName => _partName;
    public Dimensions Dimensions => _dimensions;
    public Quantity Quantity => _quantity;
    public string? AdditionalInfo => _additionalInfo;

    private OrderFurniturePartLine()
    {
    }

    internal static OrderFurniturePartLine Create(
        PartName partName,
        Dimensions dimensions,
        Quantity quantity,
        string? additionalInfo = null)
    {
        if (quantity.Value <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

        return new OrderFurniturePartLine
        {
            _partName = partName ?? throw new ArgumentNullException(nameof(partName)),
            _dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions)),
            _quantity = quantity ?? throw new ArgumentNullException(nameof(quantity)),
            _additionalInfo = additionalInfo
        };
    }

    internal void Update(PartName partName, Dimensions dimensions, Quantity quantity, string? additionalInfo)
    {
        if (quantity.Value <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

        _partName = partName ?? throw new ArgumentNullException(nameof(partName));
        _dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
        _quantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
        _additionalInfo = additionalInfo;
    }
}