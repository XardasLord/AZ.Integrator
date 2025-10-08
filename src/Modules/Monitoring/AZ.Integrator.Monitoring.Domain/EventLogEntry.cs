using System.Text.Json;
using AZ.Integrator.Domain.SeedWork;

namespace AZ.Integrator.Monitoring.Domain;

public class EventLogEntry : Entity<long>, IAggregateRoot
{
    public required Guid TenantId { get; set; }
    public string EventName { get; set; } = null!;
    public string? EventType { get; set; }
    public required string SourceSystemId { get; set; }
    public required string SourceModule { get; set; }
    public string? ReferenceId { get; set; }
    public string? ReferenceNumber { get; set; }
    public JsonDocument? Payload { get; set; }
    public required Guid CreatedById { get; set; }
    public required string CreatedByName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CorrelationId { get; set; }
    public JsonDocument? Metadata { get; set; }
}