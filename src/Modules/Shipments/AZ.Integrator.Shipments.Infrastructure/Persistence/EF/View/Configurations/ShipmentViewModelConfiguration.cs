using AZ.Integrator.Shipments.Contracts.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shipments.Infrastructure.Persistence.EF.View.Configurations;

public class ShipmentViewModelConfiguration : IEntityTypeConfiguration<ShipmentViewModel>
{
    public void Configure(EntityTypeBuilder<ShipmentViewModel> builder)
    {
        builder.ToView("shipments_view");
        builder.HasNoKey();
        
        builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.ShipmentNumber).HasColumnName("shipment_number");
        builder.Property(x => x.ExternalOrderNumber).HasColumnName("external_order_number");
        builder.Property(x => x.ShipmentProvider).HasColumnName("shipment_provider");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
    }
}