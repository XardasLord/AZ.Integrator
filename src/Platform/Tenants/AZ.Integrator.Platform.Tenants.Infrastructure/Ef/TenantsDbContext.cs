using AZ.Integrator.Platform.FeatureFlags.Infrastructure.Entities;
using AZ.Integrator.Platform.Tenants.Infrastructure.Entities;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Platform.Tenants.Infrastructure.Ef;

public class TenantsDbContext(DbContextOptions<TenantsDbContext> options) : DbContext(options)
{
    public DbSet<Tenant> Tenants => Set<Tenant>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Tenant>(e =>
        {
            e.ToTable("tenants", SchemaDefinition.Platform);
            
            e.HasKey(x => x.TenantId);
            
            e.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired();
            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
            e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            e.Property(x => x.ModifiedAt).HasColumnName("modified_at").IsRequired(false);

            e.HasMany(x => x.FeatureFlags)
                .WithOne()
                .HasForeignKey(x => x.TenantId);
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
        });
    }
}