using AZ.Integrator.Procurement.Contracts.Suppliers;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Procurement.Infrastructure.Persistence.EF.View.Configurations;

public class SupplierMailboxViewModelConfiguration : IEntityTypeConfiguration<SupplierMailboxViewModel>
{
    public void Configure(EntityTypeBuilder<SupplierMailboxViewModel> builder)
    {
        builder.ToView("supplier_mailboxes_view", SchemaDefinition.Procurement);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");
            
        builder.Property(x => x.SupplierId)
            .HasColumnName("supplier_id");
            
        builder.Property(x => x.Email)
            .HasColumnName("email");
    }
}