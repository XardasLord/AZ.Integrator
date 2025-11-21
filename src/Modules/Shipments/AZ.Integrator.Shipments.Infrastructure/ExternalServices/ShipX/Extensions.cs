using AZ.Integrator.Shared.Infrastructure.ExternalServices;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shipments.Infrastructure.ExternalServices.ShipX;

public static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:ShipX";
    
    public static IServiceCollection AddShipX(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ShipXOptions>(configuration.GetRequiredSection(OptionsSectionName));
        
        var shipXOptions = configuration.GetOptions<ShipXOptions>(OptionsSectionName);

        services.AddTransient<IShipXService, ShipXApiService>();
            
        services.AddHttpClient(ExternalHttpClientNames.ShipXHttpClientName, config =>
        {
            config.BaseAddress = new Uri(shipXOptions.ApiUrl);
            config.Timeout = new TimeSpan(0, 0, 20);
        });
        
        return services;
    }
}