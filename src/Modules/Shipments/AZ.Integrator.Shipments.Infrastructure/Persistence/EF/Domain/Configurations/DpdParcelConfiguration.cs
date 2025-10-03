using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shipments.Infrastructure.Persistence.EF.Domain.Configurations;

public class DpdParcelConfiguration : IEntityTypeConfiguration<DpdParcel>
{
    public void Configure(EntityTypeBuilder<DpdParcel> builder)
    {
        builder.ToTable("dpd_parcels");

        builder.Ignore(e => e.Events);
        builder.HasKey(e => e.Number);

        builder.Property(e => e.Number)
            .HasColumnName("number")
            .HasConversion(number => number.Value, number => new ParcelNumber(number))
            .IsRequired();
        
        builder.Property<PackageNumber>("_packageId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("package_id")
            .IsRequired();

        builder.Property(e => e.Waybill)
            .HasColumnName("waybill")
            .HasConversion(number => number.Value, number => new Waybill(number));
    }
}