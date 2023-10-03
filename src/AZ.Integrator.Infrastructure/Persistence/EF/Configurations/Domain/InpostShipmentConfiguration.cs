﻿using AZ.Integrator.Domain.Aggregates.InpostShipment;
using AZ.Integrator.Domain.Aggregates.InpostShipment.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Infrastructure.Persistence.EF.Configurations.Domain;

public class InpostShipmentConfiguration : IEntityTypeConfiguration<InpostShipment>
{
    public void Configure(EntityTypeBuilder<InpostShipment> builder)
    {
        builder.ToTable("inpost_shipments");

        builder.HasKey(e => e.Number);

        builder.Property(e => e.Number)
            .HasColumnName("number")
            .HasConversion(number => number.Value, number => new ShipmentNumber(number));

        builder.Property(e => e.AllegroAllegroOrderNumber)
            .HasColumnName("allegro_order_number")
            .HasConversion(number => number.Value, number => new AllegroOrderNumber(number));
        
        builder.OwnsOne(e => e.CreationInformation, ci =>
        {
            ci.Property(c => c.CreatedAt).HasColumnName("created_at").IsRequired();
            ci.Property(c => c.CreatedBy).HasColumnName("created_by").IsRequired();
        });

        builder.Navigation(x => x.CreationInformation).IsRequired();
    }
}