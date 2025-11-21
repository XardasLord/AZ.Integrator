using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Integrations.Infrastructure.Persistence.EF.View.Configurations;

public class AllegroIntegrationViewModelConfiguration : IEntityTypeConfiguration<AllegroIntegrationViewModel>
{
    public void Configure(EntityTypeBuilder<AllegroIntegrationViewModel> builder)
    {
        builder.ToView("allegro_view", SchemaDefinition.Integration);

        builder.HasKey(e => new { e.TenantId, e.SourceSystemId });

        builder.Property(e => e.TenantId).HasColumnName("tenant_id");
        builder.Property(e => e.SourceSystemId).HasColumnName("source_system_id");
        builder.Property(e => e.AccessToken).HasColumnName("access_token");
        builder.Property(e => e.RefreshToken).HasColumnName("refresh_token");
        builder.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        builder.Property(e => e.ClientId).HasColumnName("client_id");
        builder.Property(e => e.ClientSecret).HasColumnName("client_secret");
        builder.Property(e => e.RedirectUri).HasColumnName("redirect_uri");
        builder.Property(e => e.DisplayName).HasColumnName("display_name");
        builder.Property(e => e.IsEnabled).HasColumnName("is_enabled");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
    }
}