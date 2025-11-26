using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Integrations.Infrastructure.Persistence.EF.View.Configurations;

public class ErliIntegrationViewModelConfiguration : IEntityTypeConfiguration<ErliIntegrationViewModel>
{
    public void Configure(EntityTypeBuilder<ErliIntegrationViewModel> builder)
    {
        builder.ToView("erli_view", SchemaDefinition.Integration);

        builder.HasKey(e => new { e.TenantId, e.SourceSystemId });

        builder.Property(e => e.TenantId).HasColumnName("tenant_id");
        builder.Property(e => e.SourceSystemId).HasColumnName("source_system_id");
        builder.Property(e => e.ApiKey).HasColumnName("api_key");
        builder.Property(e => e.DisplayName).HasColumnName("display_name");
        builder.Property(e => e.IsEnabled).HasColumnName("is_enabled");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        builder.Property(e => e.DeletedBy).HasColumnName("deleted_by");
    }
}