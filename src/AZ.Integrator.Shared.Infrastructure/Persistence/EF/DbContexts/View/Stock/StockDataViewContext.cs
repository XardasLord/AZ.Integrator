using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Stock.Configurations;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Stock;

public class StockDataViewContext(DbContextOptions<StockDataViewContext> options) : DbContext(options)
{
    public virtual DbSet<StockViewModel> Stocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StockViewModelConfiguration());
        modelBuilder.ApplyConfiguration(new StockLogViewModelConfiguration());
    }
}