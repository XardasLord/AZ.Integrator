using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Catalog.Infrastructure.Persistence.EF.Domain.Configurations;

public class FurnitureModelConfiguration : IEntityTypeConfiguration<FurnitureModel>
{
    public void Configure(EntityTypeBuilder<FurnitureModel> builder)
    {
        builder.ToTable("furniture_models", SchemaDefinition.Catalog);

        builder.Ignore(e => e.Events);

        builder.HasKey(e => new { e.FurnitureCode, e.TenantId });

        builder.Property(e => e.FurnitureCode)
            .HasColumnName("furniture_code")
            .HasConversion(code => code.Value, code => new FurnitureCode(code))
            .IsRequired();

        builder.Property(e => e.TenantId)
            .HasColumnName("tenant_id")
            .HasConversion(id => id.Value, id => new TenantId(id))
            .IsRequired();

        builder.Property(e => e.Version)
            .HasColumnName("version")
            .IsRequired();

        builder.Property(e => e.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired();

        builder.Property(e => e.DeletedAt)
            .HasColumnName("deleted_at")
            .IsRequired(false);
        
        builder.ComplexProperty(e => e.CreationInformation, ci =>
        {
            ci.Property(c => c.CreatedAt).HasColumnName("created_at").IsRequired();
            ci.Property(c => c.CreatedBy).HasColumnName("created_by").IsRequired();
            ci.Property(c => c.TenantId)
                .HasConversion(id => id.Value, id => new TenantId(id))
                .HasColumnName("tenant_id")
                .IsRequired();

            ci.IsRequired();
        });
        
        builder.ComplexProperty(e => e.ModificationInformation, mi =>
        {
            mi.Property(c => c.ModifiedAt).HasColumnName("modified_at").IsRequired();
            mi.Property(c => c.ModifiedBy).HasColumnName("modified_by").IsRequired();

            mi.IsRequired();
        });

        builder.HasMany(x => x.PartDefinitions)
            .WithOne()
            .HasForeignKey("FurnitureCode", "TenantId")
            .OnDelete(DeleteBehavior.Cascade);

        // Global query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
