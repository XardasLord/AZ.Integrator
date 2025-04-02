namespace AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View;

public class StockViewModel
{
    public string PackageCode { get; set; }
    public int Quantity { get; set; }
    public ICollection<StockLogViewModel> Logs { get; set; }
}

public class StockLogViewModel
{
    public int Id { get; set; }
    public string PackageCode { get; set; }
    public int ChangeQuantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
}