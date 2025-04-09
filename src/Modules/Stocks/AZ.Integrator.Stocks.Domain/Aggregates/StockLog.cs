using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Stocks.Domain.Aggregates.ValueObjects;

namespace AZ.Integrator.Stocks.Domain.Aggregates;

public class StockLog : Entity<StockLogId>
{
    private PackageCode _packageCode;
    private ChangeQuantity _changeQuantity;
    private OperatorCreationInformation _creationInformation;
    
    public ChangeQuantity ChangeQuantity => _changeQuantity;
    public OperatorCreationInformation CreationInformation => _creationInformation;
    
    private StockLog() { }
    
    internal StockLog(PackageCode packageCode, ChangeQuantity changeQuantity, string operatorLogin, Guid operatorId, DateTime createdAt)
    {
        _packageCode = packageCode;
        _changeQuantity = changeQuantity;
        _creationInformation = new OperatorCreationInformation(createdAt, operatorLogin, operatorId);
    }
}