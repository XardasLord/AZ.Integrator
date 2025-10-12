using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;
using AZ.Integrator.Domain.Abstractions;

namespace AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;

public class FurnitureModel : Entity, IAggregateRoot
{
    private FurnitureCode _furnitureCode = null!;
    private TenantId _tenantId;
    private int _version;
    private bool _isDeleted;
    private DateTime? _deletedAt;
    private TenantCreationInformation _creationInformation;
    private readonly List<PartDefinition> _partDefinitions;

    public FurnitureCode FurnitureCode => _furnitureCode;
    public TenantId TenantId => _tenantId;
    public int Version => _version;
    public bool IsDeleted => _isDeleted;
    public DateTime? DeletedAt => _deletedAt;
    public TenantCreationInformation CreationInformation => _creationInformation;
    public IReadOnlyCollection<PartDefinition> PartDefinitions => _partDefinitions.AsReadOnly();

    private FurnitureModel()
    {
        _partDefinitions = [];
    }

    public static FurnitureModel Create(FurnitureCode furnitureCode, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        return new FurnitureModel
        {
            _furnitureCode = furnitureCode ?? throw new ArgumentNullException(nameof(furnitureCode)),
            _tenantId = currentUser.TenantId,
            _version = 1,
            _isDeleted = false,
            _creationInformation = new TenantCreationInformation(
                currentDateTime.CurrentDate(), 
                currentUser.UserId, 
                currentUser.TenantId)
        };
    }

    public void AddPartDefinition(PartName name, Dimensions dimensions, Color color)
    {
        if (_isDeleted)
            throw new InvalidOperationException("Cannot add part definition to deleted furniture model");

        var partDefinition = new PartDefinition(name, dimensions, color);
        _partDefinitions.Add(partDefinition);
        _version++;
    }

    public void UpdatePartDefinition(Guid partDefinitionId, PartName name, Dimensions dimensions, Color color)
    {
        if (_isDeleted)
            throw new InvalidOperationException("Cannot update part definition in deleted furniture model");

        var partDefinition = _partDefinitions.FirstOrDefault(p => p.Id == partDefinitionId)
            ?? throw new ArgumentException($"Part definition with ID {partDefinitionId} not found");

        partDefinition.UpdateName(name);
        partDefinition.UpdateDimensions(dimensions);
        partDefinition.UpdateColor(color);
        _version++;
    }

    public void RemovePartDefinition(Guid partDefinitionId)
    {
        if (_isDeleted)
            throw new InvalidOperationException("Cannot remove part definition from deleted furniture model");

        var partDefinition = _partDefinitions.FirstOrDefault(p => p.Id == partDefinitionId)
            ?? throw new ArgumentException($"Part definition with ID {partDefinitionId} not found");

        _partDefinitions.Remove(partDefinition);
        _version++;
    }

    public void Delete(ICurrentDateTime currentDateTime)
    {
        _isDeleted = true;
        _deletedAt = currentDateTime.CurrentDate();
        _version++;
    }

    public void Restore()
    {
        _isDeleted = false;
        _deletedAt = null;
        _version++;
    }
}