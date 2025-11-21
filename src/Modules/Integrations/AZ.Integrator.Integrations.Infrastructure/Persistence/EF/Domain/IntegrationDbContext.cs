using AZ.Integrator.Integrations.Domain.Aggregates.Allegro;
using AZ.Integrator.Integrations.Domain.Aggregates.Erli;
using AZ.Integrator.Integrations.Domain.Aggregates.Fakturownia;
using AZ.Integrator.Integrations.Domain.Aggregates.Inpost;
using AZ.Integrator.Integrations.Domain.Aggregates.Shopify;
using AZ.Integrator.Integrations.Infrastructure.Persistence.EF.Domain.Configurations;
using AZ.Integrator.Shared.Infrastructure.Mediator;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Integrations.Infrastructure.Persistence.EF.Domain;

public class IntegrationDbContext(DbContextOptions<IntegrationDbContext> options, IMediator mediator) 
    : DbContext(options)
{
    public DbSet<AllegroIntegration> Allegro { get; set; }
    public DbSet<ErliIntegration> Erli { get; set; }
    public DbSet<ShopifyIntegration> Shopify { get; set; }
    
    public DbSet<InpostIntegration> Inpost { get; set; }
    
    public DbSet<FakturowniaIntegration> Fakturownia { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AllegroIntegrationConfiguration());
        modelBuilder.ApplyConfiguration(new ErliIntegrationConfiguration());
        modelBuilder.ApplyConfiguration(new ShopifyIntegrationConfiguration());
        
        modelBuilder.ApplyConfiguration(new InpostIntegrationConfiguration());
        
        modelBuilder.ApplyConfiguration(new FakturowniaIntegrationConfiguration());

        base.OnModelCreating(modelBuilder);
    }
    
    public async Task SaveAggregateAsync(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
        await mediator.DispatchDomainEventsAsync(this);
    }
}
