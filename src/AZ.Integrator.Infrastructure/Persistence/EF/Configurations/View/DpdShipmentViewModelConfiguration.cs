﻿using AZ.Integrator.Infrastructure.Persistence.EF.Configurations.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Infrastructure.Persistence.EF.Configurations.View;

public class DpdShipmentViewModelConfiguration : IEntityTypeConfiguration<DpdShipmentViewModel>
{
    public void Configure(EntityTypeBuilder<DpdShipmentViewModel> builder)
    {
        builder.ToView("dpd_shipments_view");
        builder.HasNoKey();

        builder.Property(x => x.ShipmentNumber)
            .HasColumnName("session_id");
        
        builder.Property(x => x.AllegroOrderNumber)
            .HasColumnName("allegro_order_number");

        builder.Ignore(x => x.TrackingNumber);
        
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");
    }
}