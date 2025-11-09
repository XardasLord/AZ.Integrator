using AZ.Integrator.Procurement.Contracts.PartDefinitionOrders;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Procurement.Infrastructure.Persistence.EF.View.Configurations;

public class OrderFurnitureLineViewModelConfiguration : IEntityTypeConfiguration<OrderFurnitureLineViewModel>
{
    public void Configure(EntityTypeBuilder<OrderFurnitureLineViewModel> builder)
    {
        builder.ToView("order_furniture_lines_view", SchemaDefinition.Procurement);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");
            
        builder.Property(x => x.TenantId)
            .HasColumnName("tenant_id");
            
        builder.Property(x => x.OrderId)
            .HasColumnName("order_id");
            
        builder.Property(x => x.FurnitureCode)
            .HasColumnName("furniture_code");
        
        builder.Property(x => x.FurnitureVersion)
            .HasColumnName("furniture_version");
        
        builder.Property(x => x.QuantityOrdered)
            .HasColumnName("quantity_ordered");
        
        builder.HasMany(x => x.PartLines)
            .WithOne()
            .HasForeignKey(x => x.OrderFurnitureLineId);
    }
}