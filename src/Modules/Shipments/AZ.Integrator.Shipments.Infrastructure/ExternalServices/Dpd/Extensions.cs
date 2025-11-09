using AZ.Integrator.Shipments.Application.Common.ExternalServices.Dpd;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Dpd;

public static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:Dpd";
    
    // TODO: Move Shipments infrastructure module
    public static IServiceCollection AddDpd(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DpdOptions>(configuration.GetRequiredSection(OptionsSectionName));

        services.AddTransient<IDpdService, DpdApiService>();
        
        return services;
    }
}