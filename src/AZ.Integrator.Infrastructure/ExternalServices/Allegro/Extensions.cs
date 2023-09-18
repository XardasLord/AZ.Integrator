using AZ.Integrator.Application.Common.ExternalServices.Allegro;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Infrastructure.ExternalServices.Allegro;

public static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:Allegro";
    
    public static IServiceCollection AddAllegro(this IServiceCollection services, IConfiguration configuration)
    {
        var allegroOptions = new AllegroSettings();
        configuration.Bind(OptionsSectionName, allegroOptions);
        services.AddSingleton(allegroOptions);
        
        return services.AddTransient<IAllegroService, AllegroApiService>();
    }
}