namespace AZ.Integrator.Shared.Application.BackgroundJobs;

public interface IIntegratorRecurringJob
{
    string CronExpression { get; }
    string JobId { get; }
    Task Execute(CancellationToken cancellationToken);
}