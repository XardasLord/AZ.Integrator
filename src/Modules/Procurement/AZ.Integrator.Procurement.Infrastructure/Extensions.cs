using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Procurement.Application;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier;
using AZ.Integrator.Procurement.Infrastructure.Persistence.EF;
using AZ.Integrator.Procurement.Infrastructure.Persistence.EF.Domain;
using AZ.Integrator.Shared.Infrastructure.Repositories;
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
        services.AddModulePostgres(configuration);
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
        return services
            .AddScoped(typeof(IAggregateRepository<Supplier>), typeof(AggregateRepository<Supplier, ProcurementDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<Supplier>), typeof(AggregateReadRepository<Supplier, ProcurementDbContext>));
    }
}
