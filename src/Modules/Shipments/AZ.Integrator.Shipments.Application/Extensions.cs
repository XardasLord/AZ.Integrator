using System.Reflection;
using AZ.Integrator.Shipments.Application.Common.AutoMapper;
using AZ.Integrator.Shipments.Application.Common.Configurations;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AZ.Integrator.Shipments.Application;

public static class Extensions
{
    private const string OptionsSectionName = "Application:Inpost";
    
    public static IServiceCollection AddShipmentsModuleApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<InpostSenderOptions>(configuration.GetSection(OptionsSectionName));
        
        services
            .AddAutoMapper(config =>
            {
                config.AddProfile(new ShipmentMapper(services.BuildServiceProvider().GetRequiredService<IOptions<InpostSenderOptions>>()));
            })
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
}