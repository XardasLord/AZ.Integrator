using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Integrations.Infrastructure.Persistence.EF.View.Configurations;

public class InpostIntegrationViewModelConfiguration : IEntityTypeConfiguration<InpostIntegrationViewModel>
{
    public void Configure(EntityTypeBuilder<InpostIntegrationViewModel> builder)
    {
        builder.ToView("inpost_view", SchemaDefinition.Integration);

        builder.HasKey(e => new { e.TenantId, e.OrganizationId });

        builder.Property(e => e.TenantId).HasColumnName("tenant_id");
        builder.Property(e => e.OrganizationId).HasColumnName("organization_id");
        builder.Property(e => e.AccessToken).HasColumnName("access_token");
        builder.Property(e => e.DisplayName).HasColumnName("display_name");
        builder.Property(e => e.IsEnabled).HasColumnName("is_enabled");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
    }
}