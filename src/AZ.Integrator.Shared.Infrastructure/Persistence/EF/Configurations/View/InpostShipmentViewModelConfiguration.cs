using AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View;

public class InpostShipmentViewModelConfiguration : IEntityTypeConfiguration<InpostShipmentViewModel>
{
    public void Configure(EntityTypeBuilder<InpostShipmentViewModel> builder)
    {
        builder.ToView("inpost_shipments_view");
        builder.HasNoKey();

        builder.Property(x => x.ShipmentNumber)
            .HasColumnName("number");
        
        builder.Property(x => x.AllegroOrderNumber)
            .HasColumnName("allegro_order_number");
        
        builder.Property(x => x.TrackingNumber)
            .HasColumnName("tracking_number");
        
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");
    }
}