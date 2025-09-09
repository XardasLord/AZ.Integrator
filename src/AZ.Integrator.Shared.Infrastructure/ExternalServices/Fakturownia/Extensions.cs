using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using AZ.Integrator.Invoices.Application.Facade;
using AZ.Integrator.Invoices.Contracts;
using AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder.Strategy;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
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

        // TODO: Move to separate module
        services.AddTransient<IInvoiceService, FakturowniaService>();
        services.AddTransient<IInvoicesFacade, InvoicesFacade>();
        services.AddTransient<IInvoiceDraftBuilder, AllegroInvoiceDraftBuilder>();
        services.AddTransient<IInvoiceDraftBuilder, ErliInvoiceDraftBuilder>();
        services.AddTransient<IInvoiceDraftBuilder, ShopifyInvoiceDraftBuilder>();
        
        services.AddHttpClient(ExternalHttpClientNames.FakturowniaHttpClientName, config =>
        {
            config.BaseAddress = new Uri(fakturowniaOptions.ApiUrl);
            config.Timeout = new TimeSpan(0, 0, 20);
        });
        
        return services;
    }
}