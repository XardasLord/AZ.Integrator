using AZ.Integrator.Shared.Application.BackgroundJobs;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.Hangfire;

public class IntegratorJobConfiguration
{
    private readonly IServiceCollection _services;

    internal IntegratorJobConfiguration(IServiceCollection services)
    {
        _services = services;
    }

    public IntegratorJobConfiguration AddRecurringJob<TJob>() where TJob : IIntegratorRecurringJob
    {
        _services.AddSingleton(typeof(IIntegratorRecurringJob), typeof(TJob));

        return this;
    }
}