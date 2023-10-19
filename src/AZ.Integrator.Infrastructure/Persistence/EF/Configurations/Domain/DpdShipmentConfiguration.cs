using AZ.Integrator.Domain.Aggregates.DpdShipment;
using AZ.Integrator.Domain.Aggregates.DpdShipment.ValueObjects;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Infrastructure.Persistence.EF.Configurations.Domain;

public class DpdShipmentConfiguration : IEntityTypeConfiguration<DpdShipment>
{
    public void Configure(EntityTypeBuilder<DpdShipment> builder)
    {
        builder.ToTable("dpd_shipments");

        builder.Ignore(e => e.Events);
        builder.HasKey(e => e.SessionNumber);

        builder.Property(e => e.SessionNumber)
            .HasColumnName("session_id")
            .HasConversion(number => number.Value, number => new SessionNumber(number));

        builder.Property(e => e.AllegroAllegroOrderNumber)
            .HasColumnName("allegro_order_number")
            .HasConversion(number => number.Value, number => new AllegroOrderNumber(number));
        
        builder.OwnsOne(e => e.CreationInformation, ci =>
        {
            ci.Property(c => c.CreatedAt).HasColumnName("created_at").IsRequired();
            ci.Property(c => c.CreatedBy).HasColumnName("created_by").IsRequired();
        });

        builder.Navigation(e => e.CreationInformation).IsRequired();

        builder.HasMany(e => e.Packages)
            .WithOne()
            .HasForeignKey("_shipmentId");
    }
}