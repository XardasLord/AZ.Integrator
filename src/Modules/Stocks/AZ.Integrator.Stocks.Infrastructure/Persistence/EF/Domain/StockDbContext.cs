using AZ.Integrator.Shared.Infrastructure.Mediator;
using AZ.Integrator.Stocks.Domain.Aggregates.Stock;
using AZ.Integrator.Stocks.Domain.Aggregates.StockGroup;
using AZ.Integrator.Stocks.Infrastructure.Persistence.EF.Domain.Configurations;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Stocks.Infrastructure.Persistence.EF.Domain;

public class StockDbContext(DbContextOptions<StockDbContext> options, IMediator mediator)
    : DbContext(options)
{
    public virtual DbSet<Stock> Stocks { get; set; }
    public virtual DbSet<StockGroup> StockGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StockConfiguration());
        modelBuilder.ApplyConfiguration(new StockLogConfiguration());
        
        modelBuilder.ApplyConfiguration(new StockGroupConfiguration());
    }
    
    public async Task SaveAggregateAsync(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
        await mediator.DispatchDomainEventsAsync(this);
    }
}