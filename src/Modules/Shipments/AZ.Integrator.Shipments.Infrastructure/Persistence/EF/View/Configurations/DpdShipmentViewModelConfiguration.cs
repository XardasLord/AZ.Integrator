using AZ.Integrator.Shipments.Contracts.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shipments.Infrastructure.Persistence.EF.View.Configurations;

public class DpdShipmentViewModelConfiguration : IEntityTypeConfiguration<DpdShipmentViewModel>
{
    public void Configure(EntityTypeBuilder<DpdShipmentViewModel> builder)
    {
        builder.ToView("dpd_shipments_view");
        builder.HasNoKey();

        builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.ShipmentNumber).HasColumnName("session_id");
        builder.Property(x => x.ExternalOrderNumber).HasColumnName("external_order_number");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
    }
}