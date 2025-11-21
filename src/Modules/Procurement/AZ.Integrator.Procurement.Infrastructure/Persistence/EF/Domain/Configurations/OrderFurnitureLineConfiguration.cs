using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier.ValueObjects;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AZ.Integrator.Procurement.Infrastructure.Persistence.EF.Domain.Configurations;

public class OrderFurnitureLineConfigurationConfiguration : IEntityTypeConfiguration<OrderFurnitureLine>
{
    public void Configure(EntityTypeBuilder<OrderFurnitureLine> builder)
    {
        builder.ToTable("order_furniture_lines", SchemaDefinition.Procurement);

        builder.Ignore(e => e.Events);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, id => new OrderFurnitureLineId(id))
            .UseIdentityColumn();
        
        builder.Property<OrderId>("_orderId")
            .HasConversion(id => id.Value, id => new OrderId(id))
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(e => e.FurnitureCode)
            .HasColumnName("furniture_code")
            .IsRequired();

        builder.Property(e => e.FurnitureVersion)
            .HasColumnName("furniture_version")
            .IsRequired();

        builder.Property(e => e.TenantId)
            .HasColumnName("tenant_id")
            .HasConversion(id => id.Value, id => new TenantId(id))
            .IsRequired();

        builder.Property(e => e.QuantityOrdered)
            .HasColumnName("quantity_ordered")
            .HasConversion(id => id.Value, id => new Quantity(id))
            .IsRequired();

        builder.HasMany(x => x.Lines)
            .WithOne()
            .HasForeignKey("_orderFurnitureLineId");
    }
}
