using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Infrastructure.AllegroAccount.Configurations;

public class AllegroAccountConfiguration : IEntityTypeConfiguration<AllegroAccountViewModel>
{
    public void Configure(EntityTypeBuilder<AllegroAccountViewModel> builder)
    {
        builder.ToTable("allegro_accounts");

        builder.HasKey(e => new { e.TenantId, e.SourceSystemId });

        builder.Property(e => e.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(e => e.SourceSystemId)
            .HasColumnName("source_system_id")
            .IsRequired();

        builder.Property(e => e.AccessToken)
            .HasColumnName("access_token")
            .IsRequired();

        builder.Property(e => e.RefreshToken)
            .HasColumnName("refresh_token")
            .IsRequired();

        builder.Property(e => e.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();

        builder.Property(e => e.ClientId)
            .HasColumnName("client_id")
            .IsRequired();

        builder.Property(e => e.ClientSecret)
            .HasColumnName("client_secret")
            .IsRequired();

        builder.Property(e => e.RedirectUri)
            .HasColumnName("redirect_uri")
            .IsRequired();
    }
}