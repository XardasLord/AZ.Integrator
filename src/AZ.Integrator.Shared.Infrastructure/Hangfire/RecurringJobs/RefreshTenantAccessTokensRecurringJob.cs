using AZ.Integrator.Shared.Application.BackgroundJobs;
using AZ.Integrator.Shared.Infrastructure.Hangfire.Jobs;
using AZ.Integrator.Shared.Infrastructure.Hangfire.Jobs.RefreshTenantAccessToken;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.Hangfire.RecurringJobs;

public class RefreshTenantAccessTokensRecurringJob : IIntegratorRecurringJob
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public string CronExpression => Cron.Hourly(0);
    public string JobId => nameof(RefreshTenantAccessTokensRecurringJob);

    public RefreshTenantAccessTokensRecurringJob(
        IBackgroundJobClient backgroundJobClient,
        IServiceScopeFactory serviceScopeFactory)
    {
        _backgroundJobClient = backgroundJobClient;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Execute(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        
        var allegroAccountDataViewContext = scope.ServiceProvider.GetService<AllegroAccountDataViewContext>();

        var tenantAccounts = await allegroAccountDataViewContext.AllegroAccounts.ToListAsync(cancellationToken);
        
        tenantAccounts.ForEach(tenant =>
            _backgroundJobClient.Enqueue<RefreshTenantAccessTokenCommandJob>(
                job => job.Execute(new RefreshTenantAccessTokenCommand
                {
                    TenantId = tenant.TenantId
                }, null)));
    }
}