﻿using AZ.Integrator.Infrastructure.ExternalServices.Allegro;
using AZ.Integrator.Infrastructure.ExternalServices.ShipX;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Infrastructure.ExternalServices;

public static class Extensions
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddAllegro(configuration)
            .AddShipX(configuration);
    }
}