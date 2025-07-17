using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Stocks.Domain.Aggregates.Shared.ValueObjects;
using AZ.Integrator.Stocks.Domain.Aggregates.Stock.ValueObjects;

namespace AZ.Integrator.Stocks.Domain.Aggregates.Stock;

public class StockLog : Entity<StockLogId>
{
    private PackageCode _packageCode;
    private ChangeQuantity _changeQuantity;
    private StockLogStatus _status;
    private OperatorCreationInformation _creationInformation;

    public ChangeQuantity ChangeQuantity => _changeQuantity;
    public StockLogStatus Status => _status;
    public OperatorCreationInformation CreationInformation => _creationInformation;
    
    private StockLog() { }
    
    internal StockLog(PackageCode packageCode, ChangeQuantity changeQuantity, string operatorLogin, Guid operatorId, DateTime createdAt)
    {
        _packageCode = packageCode;
        _changeQuantity = changeQuantity;
        _status = StockLogStatus.Active;
        _creationInformation = new OperatorCreationInformation(createdAt, operatorLogin, operatorId);
    }

    internal void Revert()
    {
        if (_status != StockLogStatus.Active)
            throw new InvalidOperationException("Cannot revert a log that is not active.");
        
        _status = StockLogStatus.Reverted;
    }
}