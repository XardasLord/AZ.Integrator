using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.Hangfire;

public static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:Postgres";
    
    public static IntegratorJobConfiguration AddIntegratorJobManager(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PostgresOptions>(configuration.GetRequiredSection(OptionsSectionName));

        var postgresOptions = configuration.GetOptions<PostgresOptions>(OptionsSectionName);
        
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseConsole()
            .UsePostgreSqlStorage(c => c.UseNpgsqlConnection(postgresOptions.ConnectionStringHangfire)));

        services.AddHangfireServer();
            
        services.AddSingleton<IntegratorRecurringJobManager>();

        return new IntegratorJobConfiguration(services);
    }

    public static IApplicationBuilder StartIntegratorRecurringJobs(this IApplicationBuilder app)
    {
        var manager = app.ApplicationServices.CreateScope().ServiceProvider.GetService<IntegratorRecurringJobManager>();
        manager.Start();
        
        return app;
    }
    
    public static IApplicationBuilder UseIntegratorHangfire(this IApplicationBuilder app)
    {
        return app.UseHangfireDashboard("/hangfire", new DashboardOptions()
        {
            Authorization = new[] { new DashboardNoAuthorizationFilter() },
            IgnoreAntiforgeryToken = true
        });
    }
}