using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Stocks.Domain.Aggregates.ValueObjects;

namespace AZ.Integrator.Stocks.Domain.Aggregates;

public class Stock : Entity, IAggregateRoot
{
    private PackageCode _packageCode;
    private Quantity _quantity;
    private List<StockLog> _stockLogs;
    
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
}