using AZ.Integrator.Shared.Infrastructure.Mediator;
using AZ.Integrator.Stocks.Infrastructure.Persistence.EF.Domain.Configurations;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Stocks.Infrastructure.Persistence.EF.Domain;

public class StockDbContext(DbContextOptions<StockDbContext> options, IMediator mediator)
    : DbContext(options)
{
    public virtual DbSet<Stocks.Domain.Aggregates.Stock> Stocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StockConfiguration());
        modelBuilder.ApplyConfiguration(new StockLogConfiguration());
    }
    
    public async Task SaveAggregateAsync(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
        await mediator.DispatchDomainEventsAsync(this);
    }
}