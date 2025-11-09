using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Monitoring.Application;
using AZ.Integrator.Monitoring.Application.Facade;
using AZ.Integrator.Monitoring.Contracts;
using AZ.Integrator.Monitoring.Domain;
using AZ.Integrator.Monitoring.Infrastructure.Persistence.EF;
using AZ.Integrator.Monitoring.Infrastructure.Persistence.EF.Domain;
using AZ.Integrator.Shared.Infrastructure.Repositories;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Monitoring.Infrastructure;

public static class Extensions
{
    public static IServiceCollection RegisterMonitoringModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleApplication(configuration);
        services.AddModulePostgres(configuration);
        services.AddModuleDomainServices();

        services.AddTransient<IMonitoringFacade, MonitoringFacade>();
        
        return services;
    }
    
    public static IEndpointRouteBuilder MapMonitoringModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string swaggerGroupName = "Monitoring";
        
        var monitoringGroup = endpoints.MapGroup("/api/monitoring").WithTags(swaggerGroupName).RequireAuthorization();
        
        monitoringGroup.MapGet("/info", () => Results.Ok("Monitoring module")).AllowAnonymous();
        
        return endpoints;
    }
    
    public static IRequestExecutorBuilder AddMonitoringModuleGraphQlObjects(this IRequestExecutorBuilder builder)
    {
        return builder;
    }
    
    private static IServiceCollection AddModuleDomainServices(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IAggregateRepository<EventLogEntry>), typeof(AggregateRepository<EventLogEntry, MonitoringDbContext>));
    }
}