using AZ.Integrator.Integrations.Infrastructure.Hangfire.Jobs.RefreshTenantAccessToken;
using AZ.Integrator.Integrations.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Shared.Application.BackgroundJobs;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Integrations.Infrastructure.Hangfire.RecurringJobs;

public class RefreshAllegroTenantAccessTokensRecurringJob(
    IBackgroundJobClient backgroundJobClient,
    IServiceScopeFactory serviceScopeFactory)
    : IIntegratorRecurringJob
{
    public string CronExpression => Cron.Hourly(0);
    public string JobId => nameof(RefreshAllegroTenantAccessTokensRecurringJob);

    public async Task Execute(CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var integrationDataViewContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<IntegrationDataViewContext>>();

        await using var dataViewContext = await integrationDataViewContextFactory.CreateDbContextAsync(cancellationToken);
        
        var allegroAccounts = await dataViewContext.Allegro
            .Where(x => x.RefreshToken.Length > 0)
            .ToListAsync(cancellationToken);
        
        allegroAccounts.ForEach(account =>
            backgroundJobClient.Enqueue<RefreshAllegroTenantAccessTokenCommandJob>(
                job => job.Execute(new RefreshAllegroTenantAccessTokenCommand
                {
                    TenantId = account.TenantId,
                    SourceSystemId = account.SourceSystemId
                }, null)));
    }
}