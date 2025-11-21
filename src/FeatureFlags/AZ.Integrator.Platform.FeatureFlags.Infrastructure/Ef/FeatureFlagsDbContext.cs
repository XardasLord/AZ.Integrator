using AZ.Integrator.Platform.FeatureFlags.Infrastructure.Entities;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Platform.FeatureFlags.Infrastructure.Ef;

public class FeatureFlagsDbContext(DbContextOptions<FeatureFlagsDbContext> options) : DbContext(options)
{
    public DbSet<FeatureFlag> FeatureFlags => Set<FeatureFlag>();
    public DbSet<TenantFeatureFlag> TenantFeatureFlags => Set<TenantFeatureFlag>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<FeatureFlag>(e =>
        {
            e.ToTable("feature_flags", SchemaDefinition.Platform);
            e.HasKey(x => x.Code);
            e.Property(x => x.Code).HasColumnName("code").IsRequired();
            e.Property(x => x.Description).HasColumnName("description").IsRequired();
        });

        builder.Entity<TenantFeatureFlag>(e =>
        {
            e.ToTable("tenant_feature_flags", SchemaDefinition.Platform);
            e.HasKey(x => new { x.TenantId, x.Code });
            e.Property(x => x.TenantId).HasColumnName("tenant_id");
            e.Property(x => x.Code).HasColumnName("code");
            e.Property(x => x.Enabled).HasColumnName("enabled");
            e.Property(x => x.ModifiedAt).HasColumnName("modified_at");
            e.Property(x => x.ModifiedBy).HasColumnName("modified_by");
            e.HasOne<FeatureFlag>()
                .WithMany()
                .HasForeignKey(x => x.Code)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
        });
    }
}