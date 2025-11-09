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
    private ModificationInformation _modificationInformation;
    private readonly List<PartDefinition> _partDefinitions;

    public FurnitureCode FurnitureCode => _furnitureCode;
    public TenantId TenantId => _tenantId;
    public int Version => _version;
    public bool IsDeleted => _isDeleted;
    public DateTime? DeletedAt => _deletedAt;
    public TenantCreationInformation CreationInformation => _creationInformation;
    public ModificationInformation ModificationInformation => _modificationInformation;
    public IReadOnlyCollection<PartDefinition> PartDefinitions => _partDefinitions.AsReadOnly();

    private FurnitureModel()
    {
        _partDefinitions = [];
    }

    public static FurnitureModel Create(
        FurnitureCode furnitureCode,
        IEnumerable<PartDefinitionVo> partDefinitionVos, 
        ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var date = currentDateTime.CurrentDate();
        
        var furnitureModel = new FurnitureModel
        {
            _furnitureCode = furnitureCode ?? throw new ArgumentNullException(nameof(furnitureCode)),
            _tenantId = currentUser.TenantId,
            _version = 1,
            _isDeleted = false,
            _creationInformation = new TenantCreationInformation(
                date, 
                currentUser.UserId, 
                currentUser.TenantId),
            _modificationInformation = new ModificationInformation(date, currentUser.UserId)
        };

        foreach (var partDefinitionVo in partDefinitionVos)
        {
            furnitureModel.AddPartDefinition(partDefinitionVo);
        }

        return furnitureModel;
    }
    
    public void UpdatePartDefinitions(
        IEnumerable<PartDefinitionVo> partDefinitionVos,
        ICurrentUser currentUser, ICurrentDateTime currentDateTime) 
    {
        var definitionVos = partDefinitionVos.ToList();
        
        DeleteRemovedPartDefinitions(definitionVos);
        
        // Process updates and additions
        definitionVos.ForEach(pd =>
        {
            if (pd.PartDefinitionId.HasValue)
                UpdatePartDefinition(pd);
            else
                AddPartDefinition(pd);
        });

        _modificationInformation = new ModificationInformation(currentDateTime.CurrentDate(), currentUser.UserId);
        _version++;
    }
    
    private void DeleteRemovedPartDefinitions(IEnumerable<PartDefinitionVo> partDefinitionVos)
    {
        // Get existing part definition IDs
        var existingPartIds = PartDefinitions.Select(p => p.Id).ToHashSet();
        
        // Get part definition IDs from VO (only those that have IDs)
        var voPartIds = partDefinitionVos
            .Where(pd => pd.PartDefinitionId.HasValue)
            .Select(pd => pd.PartDefinitionId.Value)
            .ToHashSet();
        
        // Find part definitions to delete (exist in aggregate but not in VO)
        var partIdsToDelete = existingPartIds.Except(voPartIds).ToList();
        
        // Delete part definitions that are no longer present in the VO
        partIdsToDelete.ForEach(RemovePartDefinition);
    }

    private void AddPartDefinition(PartDefinitionVo partDefinitionVo)
    {
        if (_isDeleted)
            throw new InvalidOperationException("Cannot add part definition to deleted furniture model");

        var partDefinition = partDefinitionVo.ToPartDefinitionDomain();
        
        _partDefinitions.Add(partDefinition);
    }

    private void UpdatePartDefinition(PartDefinitionVo partDefinitionVo)
    {
        if (_isDeleted)
            throw new InvalidOperationException("Cannot update part definition in deleted furniture model");

        var partDefinition = _partDefinitions.FirstOrDefault(p => p.Id == partDefinitionVo.PartDefinitionId)
            ?? throw new ArgumentException($"Part definition with ID {partDefinitionVo.PartDefinitionId} not found");

        partDefinition.UpdateName(partDefinitionVo.Name);
        partDefinition.UpdateDimensions(partDefinitionVo.Dimensions);
        partDefinition.UpdateQuantity(partDefinitionVo.Quantity);
        partDefinition.UpdateAdditionalInfo(partDefinitionVo.AdditionalInfo);
    }

    private void RemovePartDefinition(int partDefinitionId)
    {
        if (_isDeleted)
            throw new InvalidOperationException("Cannot remove part definition from deleted furniture model");

        var partDefinition = _partDefinitions.FirstOrDefault(p => p.Id == partDefinitionId)
            ?? throw new ArgumentException($"Part definition with ID {partDefinitionId} not found");

        _partDefinitions.Remove(partDefinition);
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