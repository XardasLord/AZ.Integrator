using AZ.Integrator.Invoices.Application.Common.ExternalServices.SubiektGT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.SubiektGT;

public static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:Subiekt";
    
    public static IServiceCollection AddSubiekt(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SubiektOptions>(configuration.GetRequiredSection(OptionsSectionName));

        services.AddTransient<ISubiektService, SubiektService>();
        
        return services;
    }
}