using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Erli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Erli;

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