using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Integrations.Domain.Aggregates.Shopify;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Integrations.Infrastructure.Persistence.EF.Domain.Configurations;

public class ShopifyIntegrationConfiguration : IEntityTypeConfiguration<ShopifyIntegration>
{
    public void Configure(EntityTypeBuilder<ShopifyIntegration> builder)
    {
        builder.ToTable("shopify", SchemaDefinition.Integration);

        builder.HasKey(e => new { e.TenantId, e.SourceSystemId });

        builder.Property(e => e.TenantId)
            .HasConversion(value => value.Value, value => new TenantId(value))
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(e => e.SourceSystemId)
            .HasConversion(value => value.Value, value => new SourceSystemId(value))
            .HasColumnName("source_system_id")
            .IsRequired();

        builder.Property(e => e.ApiUrl)
            .HasColumnName("api_url")
            .IsRequired();

        builder.Property(e => e.AdminApiToken)
            .HasColumnName("admin_api_token")
            .IsRequired();

        builder.Property(e => e.DisplayName)
            .HasColumnName("display_name")
            .IsRequired();

        builder.Property(e => e.IsEnabled)
            .HasColumnName("is_enabled")
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}