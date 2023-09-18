using AZ.Integrator.Application.Common.ExternalServices.Allegro;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Infrastructure.ExternalServices;

public static class Extensions
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        return services
            .AddTransient<IAllegroService, AllegroApiService>();
    }
}