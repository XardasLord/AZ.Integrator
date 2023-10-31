using AZ.Integrator.Application.Common.ExternalServices.Dpd;
using AZ.Integrator.Application.Common.ExternalServices.SubiektGT;
using AZ.Integrator.Infrastructure.ExternalServices.Dpd;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Infrastructure.ExternalServices.SubiektGT;

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