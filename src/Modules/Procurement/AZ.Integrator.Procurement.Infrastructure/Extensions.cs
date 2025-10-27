using AZ.Integrator.Procurement.Application;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Procurement.Infrastructure;

public static class Extensions
{
    public static IServiceCollection RegisterProcurementModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleApplication();
        // services.AddModulePostgres(configuration);
        services.AddModuleDomainServices();

        return services;
    }
    
    public static IEndpointRouteBuilder MapProcurementModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string swaggerGroupName = "Procurement";
        
        return endpoints;
    }
    
    public static IRequestExecutorBuilder AddProcurementModuleGraphQlObjects(this IRequestExecutorBuilder builder)
    {
        return builder;
        // .RegisterDbContext<CatalogDataViewContext>()
        // .AddType<CatalogViewResolver>();
    }
    
    private static IServiceCollection AddModuleDomainServices(this IServiceCollection services)
    {
        return services;
        // .AddScoped(typeof(IAggregateRepository<FurnitureModel>), typeof(AggregateRepository<FurnitureModel, CatalogDbContext>))
        // .AddScoped(typeof(IAggregateReadRepository<FurnitureModel>), typeof(AggregateReadRepository<FurnitureModel, CatalogDbContext>));
    }
}
