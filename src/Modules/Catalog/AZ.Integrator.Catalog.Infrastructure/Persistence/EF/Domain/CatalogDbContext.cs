using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;
using AZ.Integrator.Catalog.Infrastructure.Persistence.EF.Domain.Configurations;
using AZ.Integrator.Shared.Infrastructure.Mediator;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Catalog.Infrastructure.Persistence.EF.Domain;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options, IMediator mediator) 
    : DbContext(options)
{
    public DbSet<FurnitureModel> FurnitureModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FurnitureModelConfiguration());
        modelBuilder.ApplyConfiguration(new PartDefinitionConfiguration());

        base.OnModelCreating(modelBuilder);
    }
    
    public async Task SaveAggregateAsync(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
        await mediator.DispatchDomainEventsAsync(this);
    }
}
