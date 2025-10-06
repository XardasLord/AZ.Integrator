using AZ.Integrator.Monitoring.Domain;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Monitoring.Infrastructure.Persistence.EF.Domain.Configurations;

public class MonitoringConfiguration : IEntityTypeConfiguration<EventLogEntry>
{
    public void Configure(EntityTypeBuilder<EventLogEntry> builder)
    {
        builder.ToTable("event_logs", SchemaDefinition.Monitoring);

        builder.Ignore(e => e.Events);
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.TenantId).HasColumnName("tenant_id");
        builder.Property(e => e.EventName).HasColumnName("event_name");
        builder.Property(e => e.EventType).HasColumnName("event_type");
        builder.Property(e => e.SourceModule).HasColumnName("source_module");
        builder.Property(e => e.ReferenceId).HasColumnName("reference_id");
        builder.Property(e => e.ReferenceNumber).HasColumnName("reference_number");
        builder.Property(e => e.Payload).HasColumnName("payload");
        builder.Property(e => e.CreatedById).HasColumnName("created_by_id");
        builder.Property(e => e.CreatedByName).HasColumnName("created_by_name");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.CorrelationId).HasColumnName("correlation_id");
        builder.Property(e => e.Metadata).HasColumnName("metadata");
    }
}