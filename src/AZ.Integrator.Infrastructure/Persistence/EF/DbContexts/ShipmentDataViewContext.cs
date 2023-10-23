using AZ.Integrator.Infrastructure.Persistence.EF.Configurations.View;
using AZ.Integrator.Infrastructure.Persistence.EF.Configurations.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Infrastructure.Persistence.EF.DbContexts;

public class ShipmentDataViewContext : DbContext
{
    public virtual DbSet<ShipmentViewModel> Shipments { get; set; }
    public virtual DbSet<InpostShipmentViewModel> InpostShipments { get; set; }
    public virtual DbSet<DpdShipmentViewModel> DpdShipments { get; set; }
    
    public ShipmentDataViewContext(DbContextOptions<ShipmentDataViewContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ShipmentViewModelConfiguration());
        modelBuilder.ApplyConfiguration(new InpostShipmentViewModelConfiguration());
        modelBuilder.ApplyConfiguration(new DpdShipmentViewModelConfiguration());
    }
}