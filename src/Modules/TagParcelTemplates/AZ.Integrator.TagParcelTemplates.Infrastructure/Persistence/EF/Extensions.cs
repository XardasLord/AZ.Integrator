using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain.ParcelTemplate;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TagParcelTemplateDbContext = AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.Domain.TagParcelTemplateDbContext;

namespace AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF;

internal static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:Postgres";

    public static IServiceCollection AddModulePostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PostgresOptions>(configuration.GetRequiredSection(OptionsSectionName));

        var postgresOptions = configuration.GetOptions<PostgresOptions>(OptionsSectionName);
        
        services.AddDbContext<TagParcelTemplateDbContext>(options =>
        {
            options.EnableDetailedErrors();
            options.UseNpgsql(postgresOptions.ConnectionStringApplication);
        });
        
        services.AddPooledDbContextFactory<TagParcelTemplateDataViewContext>(options =>
        {
            options.EnableDetailedErrors();
            options.UseNpgsql(postgresOptions.ConnectionStringApplication)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        
        // EF Core + Npgsql issue (https://www.npgsql.org/doc/types/datetime.html)
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        return services;
    }
}