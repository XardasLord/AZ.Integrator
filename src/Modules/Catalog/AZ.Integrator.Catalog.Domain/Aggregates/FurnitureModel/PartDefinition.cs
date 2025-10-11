using AZ.Integrator.Domain.SeedWork;

namespace AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;

public class PartDefinition : Entity
{
    private string _name;

    public string Name => _name;
}