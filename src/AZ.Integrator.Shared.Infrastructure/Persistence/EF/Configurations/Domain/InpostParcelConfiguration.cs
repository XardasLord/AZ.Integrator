using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.Domain;

public class InpostParcelConfiguration : IEntityTypeConfiguration<Parcel>
{
    public void Configure(EntityTypeBuilder<Parcel> builder)
    {
        builder.ToTable("inpost_parcels");

        builder.Ignore(e => e.Events);
        builder.HasKey(e => e.TrackingNumber);

        builder.Property(e => e.TrackingNumber)
            .HasColumnName("tracking_number")
            .HasConversion(number => number.Value, number => new TrackingNumber(number));
        
        builder.Property<ShipmentNumber>("_shipmentNumber")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("shipment_number")
            .IsRequired();
    }
}