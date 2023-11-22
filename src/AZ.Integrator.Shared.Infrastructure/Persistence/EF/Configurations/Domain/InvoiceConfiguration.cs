using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.Domain;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("invoices");

        builder.Ignore(e => e.Events);
        builder.HasKey(e => e.Number);

        builder.Property(e => e.Number)
            .HasColumnName("number")
            .HasConversion(number => number.Value, number => new InvoiceNumber(number));

        builder.Property(e => e.AllegroAllegroOrderNumber)
            .HasColumnName("allegro_order_number")
            .HasConversion(number => number.Value, number => new AllegroOrderNumber(number))
            .IsRequired();
        
        builder.OwnsOne(e => e.CreationInformation, ci =>
        {
            ci.Property(c => c.CreatedAt).HasColumnName("created_at").IsRequired();
            ci.Property(c => c.CreatedBy).HasColumnName("created_by").IsRequired();
        });

        builder.Navigation(e => e.CreationInformation).IsRequired();
    }
}