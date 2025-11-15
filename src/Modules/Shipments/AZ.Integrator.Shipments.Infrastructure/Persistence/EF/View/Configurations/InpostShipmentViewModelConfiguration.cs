using AZ.Integrator.Shipments.Infrastructure.Persistence.EF.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shipments.Infrastructure.Persistence.EF.View.Configurations;

public class InpostShipmentViewModelConfiguration : IEntityTypeConfiguration<InpostShipmentViewModel>
{
    public void Configure(EntityTypeBuilder<InpostShipmentViewModel> builder)
    {
        builder.ToView("inpost_shipments_view");
        builder.HasNoKey();

        builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.ShipmentNumber).HasColumnName("number");
        builder.Property(x => x.ExternalOrderNumber).HasColumnName("external_order_number");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
    }
}