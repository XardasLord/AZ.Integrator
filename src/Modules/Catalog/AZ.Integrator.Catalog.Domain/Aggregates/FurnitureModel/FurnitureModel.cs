using AZ.Integrator.Domain.SeedWork;

namespace AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;

public class FurnitureModel : Entity, IAggregateRoot
{
    private List<PartDefinition> _partDefinitions;
    
    public IReadOnlyCollection<PartDefinition> PartDefinitions => _partDefinitions;

    private FurnitureModel()
    {
        _partDefinitions = [];
    }
    
    
}