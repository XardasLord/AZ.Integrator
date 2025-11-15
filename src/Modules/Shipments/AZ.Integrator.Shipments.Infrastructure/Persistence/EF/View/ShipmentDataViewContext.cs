using AZ.Integrator.Shipments.Infrastructure.Persistence.EF.View.Configurations;
using AZ.Integrator.Shipments.Infrastructure.Persistence.EF.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shipments.Infrastructure.Persistence.EF.View;

public class ShipmentDataViewContext(DbContextOptions<ShipmentDataViewContext> options) : DbContext(options)
{
    public virtual DbSet<ShipmentViewModel> Shipments { get; set; }
    public virtual DbSet<InpostShipmentViewModel> InpostShipments { get; set; }
    public virtual DbSet<DpdShipmentViewModel> DpdShipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ShipmentViewModelConfiguration());
        modelBuilder.ApplyConfiguration(new InpostShipmentViewModelConfiguration());
        modelBuilder.ApplyConfiguration(new DpdShipmentViewModelConfiguration());
    }
}