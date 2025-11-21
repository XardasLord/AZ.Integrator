using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Integrations.Infrastructure.Persistence.EF.View.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Integrations.Infrastructure.Persistence.EF.View;

public class IntegrationDataViewContext(DbContextOptions<IntegrationDataViewContext> options) : DbContext(options)
{
    public virtual DbSet<AllegroIntegrationViewModel> Allegro { get; set; }
    public virtual DbSet<ErliIntegrationViewModel> Erli { get; set; }
    public virtual DbSet<ShopifyIntegrationViewModel> Shopify { get; set; }
    
    public virtual DbSet<InpostIntegrationViewModel> Inpost { get; set; }
    
    public virtual DbSet<FakturowniaIntegrationViewModel> Fakturownia { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AllegroIntegrationViewModelConfiguration());
        modelBuilder.ApplyConfiguration(new ErliIntegrationViewModelConfiguration());
        modelBuilder.ApplyConfiguration(new ShopifyIntegrationViewModelConfiguration());
        
        modelBuilder.ApplyConfiguration(new InpostIntegrationViewModelConfiguration());
        
        modelBuilder.ApplyConfiguration(new FakturowniaIntegrationViewModelConfiguration());
    }
}