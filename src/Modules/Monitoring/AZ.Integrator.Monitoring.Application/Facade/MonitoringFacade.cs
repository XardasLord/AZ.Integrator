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
        INotification domainEvent,
        string tenantId,
        Guid userId,
        string userName,
        DateTime dateTime,
        string sourceModule,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        var entry = new EventLogEntry
        {
            TenantId = tenantId,
            EventName = domainEvent.GetType().Name,
            EventType = DomainEventNameConst,
            SourceModule = sourceModule,
            Payload = JsonSerializer.Serialize(domainEvent),
            CreatedById = userId,
            CreatedByName = userName,
            CreatedAt = dateTime,
            CorrelationId = correlationId,
            Metadata = null // TODO: add metadata if needed
        };

        await repository.AddAsync(entry, cancellationToken);
    }
}