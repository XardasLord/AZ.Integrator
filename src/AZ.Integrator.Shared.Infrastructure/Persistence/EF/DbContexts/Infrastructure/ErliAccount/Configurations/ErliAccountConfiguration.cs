using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Infrastructure.ErliAccount.Configurations;

public class ErliAccountConfiguration : IEntityTypeConfiguration<ErliAccountViewModel>
{
    public void Configure(EntityTypeBuilder<ErliAccountViewModel> builder)
    {
        builder.ToTable("erli", SchemaDefinition.Account);

        builder.HasKey(e => new { e.TenantId, e.SourceSystemId });

        builder.Property(e => e.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(e => e.SourceSystemId)
            .HasColumnName("source_system_id")
            .IsRequired();

        builder.Property(e => e.ApiKey)
            .HasColumnName("api_key")
            .IsRequired();
    }
}