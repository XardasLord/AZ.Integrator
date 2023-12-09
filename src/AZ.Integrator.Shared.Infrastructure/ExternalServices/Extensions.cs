using AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.Dpd;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.Fakturownia;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.ShipX;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices;

public static class Extensions
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddAllegro(configuration)
            .AddShipX(configuration)
            .AddDpd(configuration)
            .AddFakturownia(configuration);
    }
}