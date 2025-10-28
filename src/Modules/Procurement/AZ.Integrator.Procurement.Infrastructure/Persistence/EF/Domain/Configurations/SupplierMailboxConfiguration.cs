using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier.ValueObjects;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Procurement.Infrastructure.Persistence.EF.Domain.Configurations;

public class SupplierMailboxConfiguration : IEntityTypeConfiguration<SupplierMailbox>
{
    public void Configure(EntityTypeBuilder<SupplierMailbox> builder)
    {
        builder.ToTable("supplier_mailboxes", SchemaDefinition.Procurement);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.Email)
            .HasColumnName("email")
            .HasConversion(email => email.Value, email => new Email(email))
            .IsRequired();

        // Foreign key properties for composite key relationship
        builder.Property<SupplierId>("SupplierId")
            .HasColumnName("supplier_id")
            .HasConversion(id => id.Value, id => new SupplierId(id))
            .IsRequired();
    }
}
