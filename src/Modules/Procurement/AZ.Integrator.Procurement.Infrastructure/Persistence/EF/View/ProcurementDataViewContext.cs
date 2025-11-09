using AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders;
using AZ.Integrator.Procurement.Contracts.PartDefinitionOrders;
using AZ.Integrator.Procurement.Contracts.Suppliers;
using AZ.Integrator.Procurement.Infrastructure.Persistence.EF.View.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Procurement.Infrastructure.Persistence.EF.View;

public class ProcurementDataViewContext(DbContextOptions<ProcurementDataViewContext> options) : DbContext(options), IProcurementDataViewContext
{
    public virtual DbSet<SupplierViewModel> Suppliers { get; set; }
    public virtual DbSet<PartDefinitionsOrderViewModel> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SupplierViewModelConfiguration());
        modelBuilder.ApplyConfiguration(new SupplierMailboxViewModelConfiguration());

        modelBuilder.ApplyConfiguration(new OrderViewModelConfiguration());
        modelBuilder.ApplyConfiguration(new OrderFurnitureLineViewModelConfiguration());
        modelBuilder.ApplyConfiguration(new OrderFurniturePartLineViewModelConfiguration());
    }
}