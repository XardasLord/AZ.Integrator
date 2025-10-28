using AZ.Integrator.Procurement.Contracts.Suppliers;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Procurement.Infrastructure.Persistence.EF.View.Configurations;

public class SupplierViewModelConfiguration : IEntityTypeConfiguration<SupplierViewModel>
{
    public void Configure(EntityTypeBuilder<SupplierViewModel> builder)
    {
        builder.ToView("suppliers_view", SchemaDefinition.Procurement);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");
            
        builder.Property(x => x.TenantId)
            .HasColumnName("tenant_id");
            
        builder.Property(x => x.Name)
            .HasColumnName("name");
            
        builder.Property(x => x.TelephoneNumber)
            .HasColumnName("telephone_number");
        
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");
        
        builder.Property(x => x.CreatedBy)
            .HasColumnName("created_by");
        
        builder.Property(x => x.ModifiedAt)
            .HasColumnName("modified_at");
        
        builder.Property(x => x.ModifiedBy)
            .HasColumnName("modified_by");
        
        builder.HasMany(x => x.Mailboxes)
            .WithOne()
            .HasForeignKey(x => x.SupplierId);
    }
}