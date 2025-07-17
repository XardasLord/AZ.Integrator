using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Stocks.Domain.Aggregates.Stock.ValueObjects;
using AZ.Integrator.Stocks.Domain.Aggregates.StockGroup.ValueObjects;

namespace AZ.Integrator.Stocks.Domain.Aggregates.Stock;

public class Stock : Entity, IAggregateRoot
{
    private PackageCode _packageCode;
    private Quantity _quantity;
    private List<StockLog> _stockLogs;
    private StockGroupId _groupId;
    
    public PackageCode PackageCode => _packageCode;
    public Quantity Quantity => _quantity;
    public IReadOnlyCollection<StockLog> StockLogs => _stockLogs;

    private Stock()
    {
        _stockLogs = [];
    }

    private Stock(PackageCode packageCode, Quantity quantity) : this()
    {
        _packageCode = packageCode;
        _quantity = quantity;
    }
    
    public static Stock Register(PackageCode packageCode, ChangeQuantity changeQuantity, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var stock = new Stock(packageCode, 0);
        
        stock.UpdateQuantity(changeQuantity, currentUser, currentDateTime);

        return stock;
    }
    
    public void UpdateQuantity(ChangeQuantity changeQuantity, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        _quantity += changeQuantity;
        _stockLogs.Add(new StockLog(PackageCode, changeQuantity, currentUser.UserName, currentUser.UserId, currentDateTime.CurrentDate()));
    }
    
    public void RevertScannedLog(StockLogId logId, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var log = _stockLogs.FirstOrDefault(x => x.Id == logId)
            ?? throw new Exception("Log not found");
        
        _quantity += log.ChangeQuantity.Revert();
        _stockLogs.Add(new StockLog(PackageCode, log.ChangeQuantity.Revert(), currentUser.UserName, currentUser.UserId, currentDateTime.CurrentDate()));
    }
    
    public void AssignToGroup(StockGroupId groupId) 
    {
        _groupId = groupId;
    }
}