using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Fakturownia;

public static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:Fakturownia";
    
    public static IServiceCollection AddFakturownia(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FakturowniaOptions>(configuration.GetRequiredSection(OptionsSectionName));

        var fakturowniaOptions = configuration.GetOptions<FakturowniaOptions>(OptionsSectionName);

        services.AddTransient<IInvoiceService, FakturowniaService>();
        
        services.AddHttpClient(ExternalHttpClientNames.FakturowniaHttpClientName, config =>
        {
            config.BaseAddress = new Uri(fakturowniaOptions.ApiUrl);
            config.Timeout = new TimeSpan(0, 0, 20);
        });
        
        return services;
    }
}