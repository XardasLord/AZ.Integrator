using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Stocks.Domain.Aggregates.Shared.ValueObjects;
using AZ.Integrator.Stocks.Domain.Aggregates.StockGroup.ValueObjects;

namespace AZ.Integrator.Stocks.Domain.Aggregates.StockGroup;

public class StockGroup : Entity<StockGroupId>, IAggregateRoot
{
    private TenantId _tenantId;
    private StockGroupName _name;
    private Description _description;
    private OperatorCreationInformation _creationInformation;
    private ModificationInformation _modificationInformation;
    
    public TenantId TenantId => _tenantId;
    public StockGroupName Name => _name;
    public Description Description => _description;
    public OperatorCreationInformation CreationInformation => _creationInformation;
    public ModificationInformation ModificationInformation => _modificationInformation;

    private StockGroup()
    {
    }

    private StockGroup(StockGroupName name, Description description, string operatorLogin, Guid operatorId, DateTime createdAt, TenantId tenantId) : this()
    {
        _name = name;
        _description = description;
        _tenantId = tenantId;
        _creationInformation = new OperatorCreationInformation(createdAt, operatorLogin, operatorId);
        _modificationInformation = new ModificationInformation(createdAt, operatorId);
    }
    
    public static StockGroup Create(StockGroupName name, Description description, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var stockGroup = new StockGroup(name, description, currentUser.UserName, currentUser.UserId, currentDateTime.CurrentDate(), currentUser.TenantId);

        return stockGroup;
    }

    public void Update(StockGroupName name, Description description, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        _name = name;
        _description = description;
        _modificationInformation = new ModificationInformation(currentDateTime.CurrentDate(), currentUser.UserId);
    }
}