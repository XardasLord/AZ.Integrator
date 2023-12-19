using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF;

internal static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:Postgres";

    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PostgresOptions>(configuration.GetRequiredSection(OptionsSectionName));

        var postgresOptions = configuration.GetOptions<PostgresOptions>(OptionsSectionName);
        
        services.AddDbContext<UserDbContext>(options =>
        {
            options.EnableDetailedErrors();
            options.UseNpgsql(postgresOptions.ConnectionStringApplication);
        });

        services.AddDbContext<ShipmentDbContext>(options =>
        {
            options.EnableDetailedErrors();
            options.UseNpgsql(postgresOptions.ConnectionStringApplication);
        });

        services.AddDbContext<InvoiceDbContext>(options =>
        {
            options.EnableDetailedErrors();
            options.UseNpgsql(postgresOptions.ConnectionStringApplication);
        });

        services.AddDbContext<AllegroAccountDbContext>(options =>
        {
            options.EnableDetailedErrors();
            options.UseNpgsql(postgresOptions.ConnectionStringApplication);
        });

        services.AddDbContext<TagParcelTemplateDbContext>(options =>
        {
            options.EnableDetailedErrors();
            options.UseNpgsql(postgresOptions.ConnectionStringApplication);
        });
        
        services.AddDbContext<ShipmentDataViewContext>(options =>
        {
            options.EnableDetailedErrors();
            options.UseNpgsql(postgresOptions.ConnectionStringApplication)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            // .UseSnakeCaseNamingConvention();
        });
        
        services.AddDbContext<InvoiceDataViewContext>(options =>
        {
            options.EnableDetailedErrors();
            options.UseNpgsql(postgresOptions.ConnectionStringApplication)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            // .UseSnakeCaseNamingConvention();
        });
        
        services.AddDbContext<AllegroAccountDataViewContext>(options =>
        {
            options.EnableDetailedErrors();
            options.UseNpgsql(postgresOptions.ConnectionStringApplication)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            // .UseSnakeCaseNamingConvention();
        });
        
        // EF Core + Npgsql issue (https://www.npgsql.org/doc/types/datetime.html)
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        return services;
    }
}