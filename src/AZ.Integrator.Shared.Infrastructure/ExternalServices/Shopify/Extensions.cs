using AZ.Integrator.Orders.Application.Common.ExternalServices.Shopify;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Shopify;

public static class Extensions
{
    // TODO: Move to Orders infrastructure module
    public static IServiceCollection AddShopify(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IShopifyService, ShopifyApiService>();
        
        return services;
    }
}