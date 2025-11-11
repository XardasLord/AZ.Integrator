using AZ.Integrator.Platform.FeatureFlags.Abstractions;
using AZ.Integrator.Platform.FeatureFlags.Infrastructure.Ef;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Platform.FeatureFlags.Infrastructure;

public static class FeatureFlagsServiceCollectionExtensions
{
    public static IServiceCollection AddFeatureFlags(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PostgresOptions>(configuration.GetRequiredSection(PostgresOptions.SectionName));
        var postgresOptions = configuration.GetOptions<PostgresOptions>(PostgresOptions.SectionName);
        
        services.AddDbContext<FeatureFlagsDbContext>(options =>
        {
            options.EnableDetailedErrors();
            options.UseNpgsql(postgresOptions.ConnectionStringApplication);
        });
        services.AddMemoryCache();
        services.AddScoped<IFeatureFlags, FeatureFlagsService>();
        services.AddScoped<FeatureFlagsService>();
        
        return services;
    }
}