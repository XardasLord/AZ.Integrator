using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;

namespace AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;

public class PartDefinition : Entity<Guid>
{
    private PartName _name;
    private Dimensions _dimensions;
    private Color _color;
    private string? _additionalInfo;

    public PartName Name => _name;
    public Dimensions Dimensions => _dimensions;
    public Color Color => _color;
    public string? AdditionalInfo => _additionalInfo;

    private PartDefinition()
    {
    }

    internal PartDefinition(PartName name, Dimensions dimensions, Color color, string? additionalInfo = null)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
        _color = color;
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

    internal void UpdateColor(Color color)
    {
        _color = color;
    }
}