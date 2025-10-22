using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Catalog.Infrastructure.Persistence.EF.View.Configurations;

public class FurnitureModelViewModelConfiguration : IEntityTypeConfiguration<FurnitureModelViewModel>
{
    public void Configure(EntityTypeBuilder<FurnitureModelViewModel> builder)
    {
        builder.ToView("furniture_models_view", SchemaDefinition.Catalog);
        builder.HasKey(x => new { x.FurnitureCode, x.TenantId });

        builder.Property(x => x.FurnitureCode)
            .HasColumnName("furniture_code");
            
        builder.Property(x => x.TenantId)
            .HasColumnName("tenant_id");
        
        builder.Property(x => x.CreatedBy)
            .HasColumnName("created_by");
        
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");
        
        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at");
        
        builder.Property(x => x.IsDeleted)
            .HasColumnName("is_deleted");
        
        builder.Property(x => x.Version)
            .HasColumnName("version");
        
        builder.HasMany(x => x.PartDefinitions)
            .WithOne()
            .HasForeignKey(x => new { x.FurnitureCode, x.TenantId });
    }
}