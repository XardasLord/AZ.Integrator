using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Integrations.Infrastructure.Persistence.EF.View.Configurations;

public class FakturowniaIntegrationViewModelConfiguration : IEntityTypeConfiguration<FakturowniaIntegrationViewModel>
{
    public void Configure(EntityTypeBuilder<FakturowniaIntegrationViewModel> builder)
    {
        builder.ToView("fakturownia_view", SchemaDefinition.Integration);

        builder.HasKey(e => new { e.TenantId, e.SourceSystemId });

        builder.Property(e => e.TenantId).HasColumnName("tenant_id");
        builder.Property(e => e.SourceSystemId).HasColumnName("source_system_id");
        builder.Property(e => e.ApiKey).HasColumnName("api_key");
        builder.Property(e => e.ApiUrl).HasColumnName("api_url");
        builder.Property(e => e.DisplayName).HasColumnName("display_name");
        builder.Property(e => e.IsEnabled).HasColumnName("is_enabled");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
    }
}