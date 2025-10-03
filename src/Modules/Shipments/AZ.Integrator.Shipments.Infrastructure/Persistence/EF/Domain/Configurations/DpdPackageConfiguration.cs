using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shipments.Infrastructure.Persistence.EF.Domain.Configurations;

public class DpdPackageConfiguration : IEntityTypeConfiguration<DpdPackage>
{
    public void Configure(EntityTypeBuilder<DpdPackage> builder)
    {
        builder.ToTable("dpd_packages");

        builder.Ignore(e => e.Events);
        builder.HasKey(e => e.Number);

        builder.Property(e => e.Number)
            .HasColumnName("number")
            .HasConversion(number => number.Value, number => new PackageNumber(number));
        
        builder.Property<SessionNumber>("_shipmentId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("shipment_id")
            .IsRequired();

        builder.HasMany(e => e.Parcels)
            .WithOne()
            .HasForeignKey("_packageId");
    }
}