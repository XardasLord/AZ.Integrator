using AZ.Integrator.Shared.Application.BackgroundJobs;
using AZ.Integrator.Shared.Infrastructure.Hangfire.Jobs.RefreshTenantAccessToken;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.AllegroAccount;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.Hangfire.RecurringJobs;

public class RefreshTenantAccessTokensRecurringJob(
    IBackgroundJobClient backgroundJobClient,
    IServiceScopeFactory serviceScopeFactory)
    : IIntegratorRecurringJob
{
    public string CronExpression => Cron.Hourly(0);
    public string JobId => nameof(RefreshTenantAccessTokensRecurringJob);

    public async Task Execute(CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        
        var allegroAccountDataViewContext = scope.ServiceProvider.GetService<AllegroAccountDataViewContext>();

        var tenantAccounts = await allegroAccountDataViewContext.AllegroAccounts
            .Where(x => x.RefreshToken.Length > 0)
            .ToListAsync(cancellationToken);
        
        tenantAccounts.ForEach(tenant =>
            backgroundJobClient.Enqueue<RefreshTenantAccessTokenCommandJob>(
                job => job.Execute(new RefreshTenantAccessTokenCommand
                {
                    TenantId = tenant.TenantId
                }, null)));
    }
}