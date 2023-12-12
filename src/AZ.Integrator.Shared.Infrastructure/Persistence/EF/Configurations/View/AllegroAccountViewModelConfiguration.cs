using AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View;

public class AllegroAccountViewModelConfiguration : IEntityTypeConfiguration<AllegroAccountViewModel>
{
    public void Configure(EntityTypeBuilder<AllegroAccountViewModel> builder)
    {
        builder.ToView("allegro_accounts_view");

        builder.HasNoKey();

        builder.Property(e => e.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(e => e.AccessToken)
            .HasColumnName("access_token")
            .IsRequired();

        builder.Property(e => e.RefreshToken)
            .HasColumnName("refresh_token")
            .IsRequired();
    }
}