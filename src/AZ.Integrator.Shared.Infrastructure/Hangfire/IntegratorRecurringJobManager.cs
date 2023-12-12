using AZ.Integrator.Shared.Application.BackgroundJobs;
using Hangfire;

namespace AZ.Integrator.Shared.Infrastructure.Hangfire;

public class IntegratorRecurringJobManager
{
    private readonly IRecurringJobManager _manager;
    private readonly IEnumerable<IIntegratorRecurringJob> _jobs;

    public IntegratorRecurringJobManager(IRecurringJobManager manager, IEnumerable<IIntegratorRecurringJob> jobs)
    {
        _manager = manager;
        _jobs = jobs;
    }

    public void Start()
    {
        foreach (var job in _jobs)
        {
            _manager.AddOrUpdate(job.JobId, () => job.Execute(CancellationToken.None), job.CronExpression);
        }
    }
}