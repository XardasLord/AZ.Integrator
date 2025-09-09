using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Invoice.Configurations;

public class InvoiceViewModelConfiguration : IEntityTypeConfiguration<InvoiceViewModel>
{
    public void Configure(EntityTypeBuilder<InvoiceViewModel> builder)
    {
        builder.ToView("invoices_view", SchemaDefinition.Billing);
        builder.HasNoKey();

        builder.Property(x => x.InvoiceId)
            .HasColumnName("external_id");

        builder.Property(x => x.InvoiceNumber)
            .HasColumnName("number");
        
        builder.Property(x => x.ExternalOrderNumber)
            .HasColumnName("external_order_number");
        
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");
    }
}