namespace AZ.Integrator.Monitoring.Contracts;

public interface IMonitoringFacade
{
    Task LogDomainEvent(
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
        CancellationToken cancellationToken = default);
}
