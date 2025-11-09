using System.Text.Json;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Monitoring.Contracts;
using AZ.Integrator.Monitoring.Domain;
using Mediator;

namespace AZ.Integrator.Monitoring.Application.Facade;

public class MonitoringFacade(IAggregateRepository<EventLogEntry> repository) : IMonitoringFacade
{
    private const string DomainEventNameConst = "DomainEvent";
    
    public async Task LogDomainEvent(
        object domainEvent,
        Guid tenantId,
        string sourceSystemId,
        Guid userId,
        string userName,
        DateTime dateTime,
        string sourceModule,
        string referenceId,
        string referenceNumber,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        var entry = new EventLogEntry
        {
            TenantId = tenantId,
            EventName = domainEvent.GetType().Name,
            EventType = DomainEventNameConst,
            SourceSystemId = sourceSystemId,
            SourceModule = sourceModule,
            Payload = JsonSerializer.SerializeToDocument(domainEvent),
            ReferenceId = referenceId,
            ReferenceNumber = referenceNumber,
            CreatedById = userId,
            CreatedByName = userName,
            CreatedAt = dateTime,
            CorrelationId = correlationId,
            Metadata = null // TODO: add metadata if needed
        };

        await repository.AddAsync(entry, cancellationToken);
    }
}