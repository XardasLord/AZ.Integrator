using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using AZ.Integrator.Invoices.Application.Facade;
using AZ.Integrator.Invoices.Contracts;
using AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder;
using AZ.Integrator.Shared.Infrastructure.ExternalServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Invoices.Infrastructure.ExternalServices.Fakturownia;

public static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:Fakturownia";
    
    public static IServiceCollection AddFakturownia(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IInvoiceService, FakturowniaService>();
        services.AddTransient<IInvoicesFacade, InvoicesFacade>();
        services.AddTransient<IInvoiceDraftBuilder, InvoiceDraftBuilder>();
        
        services.AddHttpClient(ExternalHttpClientNames.FakturowniaHttpClientName, config =>
        {
            config.Timeout = new TimeSpan(0, 0, 20);
        });
        
        return services;
    }
}