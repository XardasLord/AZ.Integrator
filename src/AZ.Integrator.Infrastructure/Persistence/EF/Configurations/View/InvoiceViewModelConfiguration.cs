using AZ.Integrator.Infrastructure.Persistence.EF.Configurations.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Infrastructure.Persistence.EF.Configurations.View;

public class InvoiceViewModelConfiguration : IEntityTypeConfiguration<InvoiceViewModel>
{
    public void Configure(EntityTypeBuilder<InvoiceViewModel> builder)
    {
        builder.ToView("invoices_view");
        builder.HasNoKey();

        builder.Property(x => x.InvoiceNumber)
            .HasColumnName("invoice_number");
        
        builder.Property(x => x.AllegroOrderNumber)
            .HasColumnName("allegro_order_number");
        
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");
    }
}