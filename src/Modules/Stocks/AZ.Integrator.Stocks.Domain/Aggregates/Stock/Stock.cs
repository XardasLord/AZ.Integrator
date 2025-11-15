using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Stocks.Domain.Aggregates.Stock.ValueObjects;
using AZ.Integrator.Stocks.Domain.Aggregates.StockGroup.ValueObjects;

namespace AZ.Integrator.Stocks.Domain.Aggregates.Stock;

public class Stock : Entity, IAggregateRoot
{
    private PackageCode _packageCode;
    private TenantId _tenantId;
    private Quantity _quantity;
    private Quantity _threshold;
    private List<StockLog> _stockLogs;
    private StockGroupId _groupId;
    
    public PackageCode PackageCode => _packageCode;
    public TenantId TenantId => _tenantId;
    public Quantity Quantity => _quantity;
    public Quantity Threshold => _threshold;
    public IReadOnlyCollection<StockLog> StockLogs => _stockLogs;

    private Stock()
    {
        _stockLogs = [];
    }

    private Stock(PackageCode packageCode, Quantity quantity, TenantId tenantId) : this()
    {
        _packageCode = packageCode;
        _quantity = quantity;
        _threshold = 10; // Default threshold, can be changed later
        _tenantId = tenantId;
    }
    
    public static Stock Register(PackageCode packageCode, ChangeQuantity changeQuantity, string scanId, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var stock = new Stock(packageCode, 0, currentUser.TenantId);
        
        stock.UpdateQuantity(changeQuantity, scanId, currentUser, currentDateTime);

        return stock;
    }
    
    public void UpdateQuantity(ChangeQuantity changeQuantity, string scanId, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        _quantity += changeQuantity;
        _stockLogs.Add(new StockLog(PackageCode, changeQuantity, scanId, currentUser.UserName, currentUser.UserId, currentDateTime.CurrentDate(), currentUser.TenantId));
    }
    
    public void RevertScannedLog(StockLogId logId, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var log = _stockLogs.FirstOrDefault(x => x.Id == logId)
            ?? throw new Exception("Log not found");
        
        _quantity += log.ChangeQuantity.Revert();
        
        log.Revert();
    }
    
    public void AssignToGroup(StockGroupId groupId) 
    {
        _groupId = groupId;
    }
    
    public void ChangeThreshold(int threshold)
    {
        _threshold = threshold;
    }
}