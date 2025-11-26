using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Integrations.Domain.Aggregates.Fakturownia;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Integrations.Infrastructure.Persistence.EF.Domain.Configurations;

public class FakturowniaIntegrationConfiguration : IEntityTypeConfiguration<FakturowniaIntegration>
{
    public void Configure(EntityTypeBuilder<FakturowniaIntegration> builder)
    {
        builder.ToTable("fakturownia", SchemaDefinition.Integration);

        builder.HasKey(e => new { e.TenantId, e.SourceSystemId });

        builder.Property(e => e.TenantId)
            .HasConversion(value => value.Value, value => new TenantId(value))
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(e => e.SourceSystemId)
            .HasConversion(value => value.Value, value => new SourceSystemId(value))
            .HasColumnName("source_system_id")
            .IsRequired();

        builder.Property(e => e.ApiKey)
            .HasColumnName("api_key")
            .IsRequired();

        builder.Property(e => e.ApiUrl)
            .HasColumnName("api_url")
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

        builder.ComplexProperty(e => e.SoftDeleteInfo, sd =>
        {
            sd.IsRequired();
            
            sd.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired();
            sd.Property(x => x.DeletedAt).HasColumnName("deleted_at");
            sd.Property(x => x.DeletedBy).HasColumnName("deleted_by");
        });
    }
}