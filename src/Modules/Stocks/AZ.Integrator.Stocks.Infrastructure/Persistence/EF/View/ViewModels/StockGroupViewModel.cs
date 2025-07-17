namespace AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View.ViewModels;

public class StockGroupViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<StockViewModel> Stocks { get; set; }
}