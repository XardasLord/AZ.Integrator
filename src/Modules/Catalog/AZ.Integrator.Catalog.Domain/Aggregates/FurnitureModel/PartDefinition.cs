using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;

namespace AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;

public class PartDefinition : Entity<int>
{
    private PartName _name;
    private Dimensions _dimensions;
    private Quantity _quantity;
    private string? _additionalInfo;

    public PartName Name => _name;
    public Dimensions Dimensions => _dimensions;
    public Quantity Quantity => _quantity;
    public string? AdditionalInfo => _additionalInfo;

    private PartDefinition()
    {
    }

    internal PartDefinition(PartName name, Dimensions dimensions, Quantity quantity, string? additionalInfo = null)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
        _quantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
        _additionalInfo = additionalInfo;
    }

    internal void UpdateName(PartName name)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
    }

    internal void UpdateDimensions(Dimensions dimensions)
    {
        _dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
    }

    internal void UpdateQuantity(Quantity quantity)
    {
        _quantity = quantity;
    }

    internal void UpdateAdditionalInfo(string? additionalInfo)
    {
        _additionalInfo = additionalInfo;
    }
}