using AZ.Integrator.Orders.Application.Common.ExternalServices.Shopify;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Orders.Infrastructure.ExternalServices.Shopify;

public static class Extensions
{
    public static IServiceCollection AddShopify(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IShopifyService, ShopifyApiService>();
        
        return services;
    }
}