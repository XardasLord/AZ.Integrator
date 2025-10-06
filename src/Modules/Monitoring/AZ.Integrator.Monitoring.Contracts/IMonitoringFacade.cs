using Mediator;

namespace AZ.Integrator.Monitoring.Contracts;

public interface IMonitoringFacade
{
    Task LogDomainEvent(INotification domainEvent,
        string tenantId,
        Guid userId,
        string userName,
        DateTime dateTime,
        string sourceModule,
        string correlationId,
        CancellationToken cancellationToken = default);
}
