using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier.ValueObjects;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Procurement.Infrastructure.Persistence.EF.Domain.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("suppliers", SchemaDefinition.Procurement);

        builder.Ignore(e => e.Events);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, id => new SupplierId(id))
            .UseIdentityColumn();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasConversion(name => name.Value, name => new SupplierName(name))
            .IsRequired();

        builder.Property(e => e.TelephoneNumber)
            .HasColumnName("telephone_number")
            .HasConversion(telephone => telephone.Value, telephone => new TelephoneNumber(telephone))
            .IsRequired(false);

        builder.Property(e => e.TenantId)
            .HasColumnName("tenant_id")
            .HasConversion(id => id.Value, id => new TenantId(id))
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

        builder.HasMany(x => x.Mailboxes)
            .WithOne()
            .HasForeignKey("SupplierId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
