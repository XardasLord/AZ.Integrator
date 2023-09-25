using System.Net.Http.Headers;
using AZ.Integrator.Application.Common.ExternalServices.ShipX;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Infrastructure.ExternalServices.ShipX;

public static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:ShipX";
    
    public static IServiceCollection AddShipX(this IServiceCollection services, IConfiguration configuration)
    {
        var shipXOptions = new ShipXOptions();
        configuration.Bind(OptionsSectionName, shipXOptions);
        services.AddSingleton(shipXOptions);

        services.AddTransient<IShipXService, ShipXApiService>();
            
        services.AddHttpClient(ExternalHttpClientNames.ShipXHttpClientName, config =>
        {
            config.BaseAddress = new Uri(shipXOptions.ApiUrl);
            config.Timeout = new TimeSpan(0, 0, 20);
            config.DefaultRequestHeaders.Clear();
            config.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", shipXOptions.AccessToken);
        });
        
        return services;
    }
}