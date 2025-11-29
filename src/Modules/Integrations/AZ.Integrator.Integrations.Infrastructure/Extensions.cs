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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        services.AddTransient<IIntegrationsWriteFacade, IntegrationsWriteFacade>();

        return services;
    }
    
    public static IEndpointRouteBuilder MapIntegrationsModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string swaggerGroupName = "Integrations";
        
        var integrationsGroup = endpoints.MapGroup("/api/integrations").WithTags(swaggerGroupName).RequireAuthorization();
        
        integrationsGroup.MapGet("/info", () => Results.Ok("Integrations module")).AllowAnonymous();
        
        MapAllegroEndpoints(integrationsGroup);
        MapErliEndpoints(integrationsGroup);
        MapShopifyEndpoints(integrationsGroup);
        MapInpostEndpoints(integrationsGroup);
        MapFakturowniaEndpoints(integrationsGroup);
        
        return endpoints;
    }

    private static void MapAllegroEndpoints(RouteGroupBuilder integrationsGroup)
    {
        integrationsGroup.MapGet("/allegro/connect", 
            ([FromQuery] Guid tenantId, IConfiguration configuration) =>
            {
                // TODO: Verify if tenantId exists if not return bad request response
                
                var props = new AuthenticationProperties
                {
                    // Tu aplikacja wróci PO zakończeniu całego flow (po callbacku, zapisaniu tokenów itd.)
                    RedirectUri = $"{configuration["Application:ClientAppUrl"]}/integrations?allegro_connected",
                    Items =
                    {
                        ["tenant_id"] = tenantId.ToString()
                    }
                };

                return Results.Challenge(
                    properties: props,
                    authenticationSchemes: [ "allegro-aboxyn" ]
                );
            })
            .AllowAnonymous();
    }

    private static void MapErliEndpoints(RouteGroupBuilder integrationsGroup)
    {
        integrationsGroup.MapPost("/erli", 
            async (AddErliIntegrationRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(new Application.UseCases.Erli.Commands.Create.CreateCommand(request), cancellationToken);
            
                return Results.Ok(response);
            });

        integrationsGroup.MapPut("/erli/{sourceSystemId}", 
            async (string sourceSystemId, UpdateErliIntegrationRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                request = request with { SourceSystemId = sourceSystemId };
                var response = await mediator.Send(new Application.UseCases.Erli.Commands.Update.UpdateCommand(request), cancellationToken);
            
                return Results.Ok(response);
            });

        integrationsGroup.MapPatch("/erli/{sourceSystemId}", 
            async (string sourceSystemId, ToggleIntegrationRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new Application.UseCases.Erli.Commands.Toggle.ToggleCommand(sourceSystemId, request), cancellationToken);
            
                return Results.NoContent();
            });

        integrationsGroup.MapDelete("/erli/{sourceSystemId}", 
            async (string sourceSystemId, IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new Application.UseCases.Erli.Commands.Delete.DeleteCommand(sourceSystemId), cancellationToken);
            
                return Results.NoContent();
            });

        integrationsGroup.MapGet("/erli/{sourceSystemId}/test", 
            async (string sourceSystemId, IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new Application.UseCases.Erli.Commands.Test.TestCommand(sourceSystemId), cancellationToken);
            
                return Results.Ok();
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

        integrationsGroup.MapPut("/shopify/{sourceSystemId}", 
            async (string sourceSystemId, UpdateShopifyIntegrationRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                request = request with { SourceSystemId = sourceSystemId };
                var response = await mediator.Send(new Application.UseCases.Shopify.Commands.Update.UpdateCommand(request), cancellationToken);
            
                return Results.Ok(response);
            });

        integrationsGroup.MapPatch("/shopify/{sourceSystemId}", 
            async (string sourceSystemId, ToggleIntegrationRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new Application.UseCases.Shopify.Commands.Toggle.ToggleCommand(sourceSystemId, request), cancellationToken);
            
                return Results.NoContent();
            });

        integrationsGroup.MapDelete("/shopify/{sourceSystemId}", 
            async (string sourceSystemId, IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new Application.UseCases.Shopify.Commands.Delete.DeleteCommand(sourceSystemId), cancellationToken);
            
                return Results.NoContent();
            });

        integrationsGroup.MapGet("/shopify/{sourceSystemId}/test", 
            async (string sourceSystemId, IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new Application.UseCases.Shopify.Commands.Test.TestCommand(sourceSystemId), cancellationToken);
            
                return Results.Ok();
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

        integrationsGroup.MapPut("/inpost/{organizationId:int}", 
            async (int organizationId, UpdateInpostIntegrationRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                request = request with { OrganizationId = organizationId };
                var response = await mediator.Send(new Application.UseCases.Inpost.Commands.Update.UpdateCommand(request), cancellationToken);
            
                return Results.Ok(response);
            });

        integrationsGroup.MapPatch("/inpost/{organizationId:int}", 
            async (int organizationId, ToggleIntegrationRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new Application.UseCases.Inpost.Commands.Toggle.ToggleCommand(organizationId, request), cancellationToken);
            
                return Results.NoContent();
            });

        integrationsGroup.MapDelete("/inpost/{organizationId:int}", 
            async (int organizationId, IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new Application.UseCases.Inpost.Commands.Delete.DeleteCommand(organizationId), cancellationToken);
            
                return Results.NoContent();
            });

        integrationsGroup.MapGet("/inpost/{organizationId:int}/test", 
            async (int organizationId, IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new Application.UseCases.Inpost.Commands.Test.TestCommand(organizationId), cancellationToken);
            
                return Results.Ok();
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

        integrationsGroup.MapPut("/fakturownia/{sourceSystemId}", 
            async (string sourceSystemId, UpdateFakturowniaIntegrationRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                request = request with { SourceSystemId = sourceSystemId };
                var response = await mediator.Send(new Application.UseCases.Fakturownia.Commands.Update.UpdateCommand(request), cancellationToken);
            
                return Results.Ok(response);
            });

        integrationsGroup.MapPatch("/fakturownia/{sourceSystemId}", 
            async (string sourceSystemId, ToggleIntegrationRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new Application.UseCases.Fakturownia.Commands.Toggle.ToggleCommand(sourceSystemId, request), cancellationToken);
            
                return Results.NoContent();
            });

        integrationsGroup.MapDelete("/fakturownia/{sourceSystemId}", 
            async (string sourceSystemId, IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new Application.UseCases.Fakturownia.Commands.Delete.DeleteCommand(sourceSystemId), cancellationToken);
            
                return Results.NoContent();
            });

        integrationsGroup.MapGet("/fakturownia/{sourceSystemId}/test", 
            async (string sourceSystemId, IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new Application.UseCases.Fakturownia.Commands.Test.TestCommand(sourceSystemId), cancellationToken);
            
                return Results.Ok();
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
