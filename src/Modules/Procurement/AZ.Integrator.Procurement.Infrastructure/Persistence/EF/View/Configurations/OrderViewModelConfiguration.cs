using AZ.Integrator.Procurement.Contracts.PartDefinitionOrders;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Procurement.Infrastructure.Persistence.EF.View.Configurations;

public class OrderViewModelConfiguration : IEntityTypeConfiguration<PartDefinitionsOrderViewModel>
{
    public void Configure(EntityTypeBuilder<PartDefinitionsOrderViewModel> builder)
    {
        builder.ToView("orders_view", SchemaDefinition.Procurement);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");
            
        builder.Property(x => x.TenantId)
            .HasColumnName("tenant_id");
            
        builder.Property(x => x.Number)
            .HasColumnName("number");
            
        builder.Property(x => x.SupplierId)
            .HasColumnName("supplier_id");

        builder.Property(e => e.AdditionalNotes)
            .HasColumnName("additional_notes");
        
        builder.Property(x => x.Status)
            .HasColumnName("status");
        
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");
        
        builder.Property(x => x.CreatedBy)
            .HasColumnName("created_by");
        
        builder.Property(x => x.ModifiedAt)
            .HasColumnName("modified_at");
        
        builder.Property(x => x.ModifiedBy)
            .HasColumnName("modified_by");
        
        builder.HasMany(x => x.FurnitureLines)
            .WithOne()
            .HasForeignKey(x => x.OrderId);
    }
}