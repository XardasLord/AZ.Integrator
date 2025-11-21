using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Integrations.Domain.Aggregates.Inpost;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Integrations.Infrastructure.Persistence.EF.Domain.Configurations;

public class InpostIntegrationConfiguration : IEntityTypeConfiguration<InpostIntegration>
{
    public void Configure(EntityTypeBuilder<InpostIntegration> builder)
    {
        builder.ToTable("inpost", SchemaDefinition.Integration);

        builder.HasKey(e => new { e.TenantId, e.OrganizationId });

        builder.Property(e => e.TenantId)
            .HasConversion(value => value.Value, value => new TenantId(value))
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(e => e.OrganizationId)
            .HasColumnName("organization_id")
            .IsRequired();

        builder.Property(e => e.AccessToken)
            .HasColumnName("access_token")
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