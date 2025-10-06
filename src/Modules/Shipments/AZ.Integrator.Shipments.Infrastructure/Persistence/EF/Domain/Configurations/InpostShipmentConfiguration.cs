using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shipments.Infrastructure.Persistence.EF.Domain.Configurations;

public class InpostShipmentConfiguration : IEntityTypeConfiguration<InpostShipment>
{
    public void Configure(EntityTypeBuilder<InpostShipment> builder)
    {
        builder.ToTable("inpost_shipments");

        builder.Ignore(e => e.Events);
        builder.HasKey(e => e.Number);

        builder.Property(e => e.Number)
            .HasColumnName("number")
            .HasConversion(number => number.Value, number => new ShipmentNumber(number));

        builder.Property(e => e.ExternalOrderNumber)
            .HasColumnName("external_order_number")
            .HasConversion(number => number.Value, number => new ExternalOrderNumber(number));
        
        builder.OwnsOne(e => e.CreationInformation, ci =>
        {
            ci.Property(c => c.CreatedAt).HasColumnName("created_at").IsRequired();
            ci.Property(c => c.CreatedBy).HasColumnName("created_by").IsRequired();
            ci.Property(c => c.TenantId)
                .HasConversion(id => id.Value, id => new TenantId(id))
                .HasColumnName("tenant_id")
                .IsRequired();
            ci.Property(c => c.SourceSystemId)
                .HasConversion(id => id.Value, id => new SourceSystemId(id))
                .HasColumnName("source_system_id")
                .IsRequired();
        });

        builder.Navigation(e => e.CreationInformation).IsRequired();

        builder.HasMany(e => e.Parcels)
            .WithOne()
            .HasForeignKey("_shipmentNumber");
    }
}