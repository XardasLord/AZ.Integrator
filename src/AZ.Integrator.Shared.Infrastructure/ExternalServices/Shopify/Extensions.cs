using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Shopify;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Shopify;

public static class Extensions
{
    public static IServiceCollection AddShopify(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IShopifyService, ShopifyApiService>();
            
        services.AddHttpClient(ExternalHttpClientNames.ShopifyHttpClientName, config =>
        {
            config.Timeout = new TimeSpan(0, 0, 20);
            config.DefaultRequestHeaders.Clear();
        });
        
        return services;
    }
}