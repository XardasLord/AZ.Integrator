using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Infrastructure.ShopifyAccount.Configurations;

public class ShopifyAccountConfiguration : IEntityTypeConfiguration<ShopifyAccountViewModel>
{
    public void Configure(EntityTypeBuilder<ShopifyAccountViewModel> builder)
    {
        builder.ToTable("shopify", SchemaDefinition.Account);

        builder.HasKey(e => new { e.TenantId, e.SourceSystemId });

        builder.Property(e => e.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(e => e.SourceSystemId)
            .HasColumnName("source_system_id")
            .IsRequired();

        builder.Property(e => e.ApiUrl)
            .HasColumnName("api_url")
            .IsRequired();

        builder.Property(e => e.AdminApiToken)
            .HasColumnName("admin_api_token")
            .IsRequired();
    }
}