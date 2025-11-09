using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice.ValueObjects;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AZ.Integrator.Invoices.Infrastructure.Persistence.EF.Domain.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoices.Domain.Aggregates.Invoice.Invoice>
{
    public void Configure(EntityTypeBuilder<Invoices.Domain.Aggregates.Invoice.Invoice> builder)
    {
        builder.ToTable("invoices", SchemaDefinition.Billing);

        builder.Ignore(e => e.Events);
        builder.HasKey(e => e.ExternalId);

        builder.Property(e => e.ExternalId)
            .HasColumnName("external_id")
            .HasConversion(id => id.Value, id => new InvoiceExternalId(id));

        builder.Property(e => e.Number)
            .HasColumnName("number")
            .HasConversion(number => number.Value, number => new InvoiceNumber(number));

        builder.Property(e => e.ExternalOrderNumber)
            .HasColumnName("external_order_number")
            .HasConversion(number => number.Value, number => new ExternalOrderNumber(number))
            .IsRequired();

        builder.Property(e => e.IdempotencyKey)
            .HasColumnName("idempotency_key")
            .HasConversion(key => key.Value, key => new IdempotencyKey(key))
            .IsRequired();
        
        builder.Property(e => e.InvoiceProvider)
            .HasColumnName("provider")
            .HasConversion(new EnumToNumberConverter<InvoiceProvider, int>())
            .IsRequired();
        
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
    }
}