using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier.ValueObjects;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AZ.Integrator.Procurement.Infrastructure.Persistence.EF.Domain.Configurations;

public class PartDefinitionsOrderConfiguration : IEntityTypeConfiguration<PartDefinitionsOrder>
{
    public void Configure(EntityTypeBuilder<PartDefinitionsOrder> builder)
    {
        builder.ToTable("orders", SchemaDefinition.Procurement);

        builder.Ignore(e => e.Events);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, id => new OrderId(id))
            .UseIdentityColumn();

        builder.Property(e => e.Number)
            .HasColumnName("number")
            .HasConversion(number => number.Value, number => new OrderNumber(number))
            .IsRequired();

        builder.Property(e => e.TenantId)
            .HasColumnName("tenant_id")
            .HasConversion(id => id.Value, id => new TenantId(id))
            .IsRequired();

        builder.Property(e => e.SupplierId)
            .HasColumnName("supplier_id")
            .HasConversion(id => id.Value, id => new SupplierId(id))
            .IsRequired();

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasConversion(new EnumToNumberConverter<OrderStatus, int>())
            .IsRequired();
        
        builder.ComplexProperty(e => e.CreationInformation, ci =>
        {
            ci.Property(c => c.CreatedAt).HasColumnName("created_at").IsRequired();
            ci.Property(c => c.CreatedBy).HasColumnName("created_by").IsRequired();
            ci.Property(c => c.TenantId)
                .HasConversion(id => id.Value, id => new TenantId(id))
                .HasColumnName("tenant_id")
                .IsRequired();

            ci.IsRequired();
        });
        
        builder.ComplexProperty(e => e.ModificationInformation, mi =>
        {
            mi.Property(c => c.ModifiedAt).HasColumnName("modified_at").IsRequired();
            mi.Property(c => c.ModifiedBy).HasColumnName("modified_by").IsRequired();

            mi.IsRequired();
        });

        builder.HasMany(x => x.FurnitureModelLines)
            .WithOne()
            .HasForeignKey("_orderId");
    }
}
