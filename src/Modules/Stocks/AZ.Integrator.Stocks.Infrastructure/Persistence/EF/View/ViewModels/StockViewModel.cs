using AZ.Integrator.Stocks.Domain.Aggregates.Stock;

namespace AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View.ViewModels;

public class StockViewModel
{
    public Guid TenantId { get; set; }
    public int? GroupId { get; set; }
    public string PackageCode { get; set; }
    public int Quantity { get; set; }
    public int Threshold { get; set; }
    public ICollection<StockLogViewModel> Logs { get; set; }
}

public class StockLogViewModel
{
    public int Id { get; set; }
    public string ScanId { get; set; }
    public Guid TenantId { get; set; }
    public string PackageCode { get; set; }
    public int ChangeQuantity { get; set; }
    public StockLogStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
}