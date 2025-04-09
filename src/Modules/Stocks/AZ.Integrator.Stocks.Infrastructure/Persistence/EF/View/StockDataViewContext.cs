using AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View;

public class StockDataViewContext(DbContextOptions<StockDataViewContext> options) : DbContext(options)
{
    public virtual DbSet<StockViewModel> Stocks { get; set; }
    public virtual DbSet<StockLogViewModel> StockLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StockViewModelConfiguration());
        modelBuilder.ApplyConfiguration(new StockLogViewModelConfiguration());
    }
}