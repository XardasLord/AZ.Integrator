using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Stocks.Domain.Aggregates.Shared.ValueObjects;
using AZ.Integrator.Stocks.Domain.Aggregates.Stock.ValueObjects;

namespace AZ.Integrator.Stocks.Domain.Aggregates.Stock;

public class StockLog : Entity<StockLogId>
{
    private PackageCode _packageCode;
    private TenantId _tenantId;
    private ChangeQuantity _changeQuantity;
    private StockLogStatus _status;
    private string _scanId;
    private OperatorCreationInformation _creationInformation;

    public TenantId TenantId => _tenantId;
    public ChangeQuantity ChangeQuantity => _changeQuantity;
    public StockLogStatus Status => _status;
    public string ScanId => _scanId;
    public OperatorCreationInformation CreationInformation => _creationInformation;
    
    private StockLog() { }
    
    internal StockLog(PackageCode packageCode, ChangeQuantity changeQuantity, string scanId, string operatorLogin, Guid operatorId, DateTime createdAt, TenantId tenantId)
    {
        _packageCode = packageCode;
        _changeQuantity = changeQuantity;
        _status = StockLogStatus.Active;
        _scanId = scanId;
        _creationInformation = new OperatorCreationInformation(createdAt, operatorLogin, operatorId);
        _tenantId = tenantId;
    }

    internal void Revert()
    {
        if (_status != StockLogStatus.Active)
            throw new InvalidOperationException("Cannot revert a log that is not active.");
        
        _status = StockLogStatus.Reverted;
    }
}