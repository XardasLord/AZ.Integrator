using AZ.Integrator.Orders.Application.Common.ExternalServices.Erli;
using AZ.Integrator.Shared.Infrastructure.ExternalServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Orders.Infrastructure.ExternalServices.Erli;

public static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:Erli";
    
    public static IServiceCollection AddErli(this IServiceCollection services, IConfiguration configuration)
    {
        var erliOptions = new ErliOptions();
        configuration.Bind(OptionsSectionName, erliOptions);
        services.AddSingleton(erliOptions);

        services.AddTransient<IErliService, ErliApiService>();
            
        services.AddHttpClient(ExternalHttpClientNames.ErliHttpClientName, config =>
        {
            config.BaseAddress = new Uri(erliOptions.ApiUrl);
            config.Timeout = new TimeSpan(0, 0, 20);
            config.DefaultRequestHeaders.Clear();
        });
        
        return services;
    }
}