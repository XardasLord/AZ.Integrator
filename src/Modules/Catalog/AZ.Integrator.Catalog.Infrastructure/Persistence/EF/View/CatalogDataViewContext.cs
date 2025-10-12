using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using AZ.Integrator.Catalog.Infrastructure.Persistence.EF.View.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Catalog.Infrastructure.Persistence.EF.View;

public class CatalogDataViewContext(DbContextOptions<CatalogDataViewContext> options) : DbContext(options)
{
    public virtual DbSet<FurnitureModelViewModel> FurnitureModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FurnitureModelViewModelConfiguration());
    }
}