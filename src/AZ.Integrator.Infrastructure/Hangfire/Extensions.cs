using AZ.Integrator.Infrastructure.Persistence.EF;
using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Infrastructure.Hangfire;

public static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:Postgres";
    
    public static IServiceCollection AddIntegratorHangfire(this IServiceCollection services, IConfiguration configuration)
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

        return services;
    }
    
    public static IApplicationBuilder UseIntegratorHangfire(this IApplicationBuilder app)
    {
        return app.UseHangfireDashboard("/hangfire", new DashboardOptions()
        {
            Authorization = new[] { new DashboardNoAuthorizationFilter() }
        });
    }
}