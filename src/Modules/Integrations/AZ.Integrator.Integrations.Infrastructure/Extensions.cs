using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Application;
using AZ.Integrator.Integrations.Contracts;
using AZ.Integrator.Integrations.Contracts.RequestDtos;
using AZ.Integrator.Integrations.Domain.Aggregates.Allegro;
using AZ.Integrator.Integrations.Domain.Aggregates.Erli;
using AZ.Integrator.Integrations.Domain.Aggregates.Fakturownia;
using AZ.Integrator.Integrations.Domain.Aggregates.Inpost;
using AZ.Integrator.Integrations.Domain.Aggregates.Shopify;
using AZ.Integrator.Integrations.Infrastructure.Facade;
using AZ.Integrator.Integrations.Infrastructure.Persistence.EF;
using AZ.Integrator.Integrations.Infrastructure.Persistence.EF.Domain;
using AZ.Integrator.Integrations.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Integrations.Infrastructure.Persistence.GraphQL.QueryResolvers;
using AZ.Integrator.Shared.Infrastructure.Repositories;
using HotChocolate.Execution.Configuration;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Integrations.Infrastructure;

public static class Extensions
{
    public static IServiceCollection RegisterIntegrationsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleApplication();
        services.AddModulePostgres(configuration);
        services.AddModuleDomainServices();

        services.AddTransient<IIntegrationsReadFacade, IntegrationsReadFacade>();

        return services;
    }
    
    public static IEndpointRouteBuilder MapIntegrationsModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string swaggerGroupName = "Integrations";
        
        var integrationsGroup = endpoints.MapGroup("/api/integrations").WithTags(swaggerGroupName).RequireAuthorization();
        
        integrationsGroup.MapGet("/info", () => Results.Ok("Integrations module")).AllowAnonymous();
        
        MapErliEndpoints(integrationsGroup);
        MapShopifyEndpoints(integrationsGroup);
        MapInpostEndpoints(integrationsGroup);
        MapFakturowniaEndpoints(integrationsGroup);
        
        return endpoints;
    }

    private static void MapErliEndpoints(RouteGroupBuilder integrationsGroup)
    {
        integrationsGroup.MapPost("/erli", 
            async (AddErliIntegrationRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(new Application.UseCases.Erli.Commands.Create.CreateCommand(request), cancellationToken);
            
                return Results.Ok(response);
            });
    }

    private static void MapShopifyEndpoints(RouteGroupBuilder integrationsGroup)
    {
        integrationsGroup.MapPost("/shopify", 
            async (AddShopifyIntegrationRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(new Application.UseCases.Shopify.Commands.Create.CreateCommand(request), cancellationToken);
            
                return Results.Ok(response);
            });
    }

    private static void MapInpostEndpoints(RouteGroupBuilder integrationsGroup)
    {
        integrationsGroup.MapPost("/inpost", 
            async (AddInpostIntegrationRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(new Application.UseCases.Inpost.Commands.Create.CreateCommand(request), cancellationToken);
            
                return Results.Ok(response);
            });
    }

    private static void MapFakturowniaEndpoints(RouteGroupBuilder integrationsGroup)
    {
        integrationsGroup.MapPost("/fakturownia", 
            async (AddFakturowniaIntegrationRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(new Application.UseCases.Fakturownia.Commands.Create.CreateCommand(request), cancellationToken);
            
                return Results.Ok(response);
            });
    }

    public static IRequestExecutorBuilder AddIntegrationsModuleGraphQlObjects(this IRequestExecutorBuilder builder)
    {
        return builder
            .RegisterDbContextFactory<IntegrationDataViewContext>()
            .AddType<IntegrationViewResolver>();
    }
    
    private static IServiceCollection AddModuleDomainServices(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IAggregateRepository<AllegroIntegration>), typeof(AggregateRepository<AllegroIntegration, IntegrationDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<AllegroIntegration>), typeof(AggregateReadRepository<AllegroIntegration, IntegrationDbContext>))
            
            .AddScoped(typeof(IAggregateRepository<ErliIntegration>), typeof(AggregateRepository<ErliIntegration, IntegrationDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<ErliIntegration>), typeof(AggregateReadRepository<ErliIntegration, IntegrationDbContext>))

            .AddScoped(typeof(IAggregateRepository<ShopifyIntegration>), typeof(AggregateRepository<ShopifyIntegration, IntegrationDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<ShopifyIntegration>), typeof(AggregateReadRepository<ShopifyIntegration, IntegrationDbContext>))

            .AddScoped(typeof(IAggregateRepository<InpostIntegration>), typeof(AggregateRepository<InpostIntegration, IntegrationDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<InpostIntegration>), typeof(AggregateReadRepository<InpostIntegration, IntegrationDbContext>))

            .AddScoped(typeof(IAggregateRepository<FakturowniaIntegration>), typeof(AggregateRepository<FakturowniaIntegration, IntegrationDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<FakturowniaIntegration>), typeof(AggregateReadRepository<FakturowniaIntegration, IntegrationDbContext>));
    }
}
