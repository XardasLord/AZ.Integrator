using AZ.Integrator.Catalog.Application;
using AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.AddPartDefinition;
using AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.Create;
using AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.Delete;
using AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.RemovePartDefinition;
using AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.UpdatePartDefinition;
using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;
using AZ.Integrator.Catalog.Infrastructure.Persistence.EF;
using AZ.Integrator.Catalog.Infrastructure.Persistence.EF.Domain;
using AZ.Integrator.Catalog.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Catalog.Infrastructure.Persistence.GraphQL.QueryResolvers;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Repositories;
using HotChocolate.Execution.Configuration;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Catalog.Infrastructure;

public static class Extensions
{
    public static IServiceCollection RegisterCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleApplication();
        services.AddModulePostgres(configuration);
        services.AddModuleDomainServices();

        return services;
    }
    
    public static IEndpointRouteBuilder MapCatalogModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string swaggerGroupName = "Catalog";
        
        var furnitureModelsGroup = endpoints.MapGroup("/api/catalog/furniture-models").WithTags(swaggerGroupName).RequireAuthorization();
        
        furnitureModelsGroup.MapGet("/info", () => Results.Ok("Furniture Models module")).AllowAnonymous();
        
        furnitureModelsGroup.MapPost("/", 
            async (CreateFurnitureModelRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new CreateCommand(request.FurnitureCode), cancellationToken);
            
            return Results.Ok(response);
        });

        furnitureModelsGroup.MapDelete("/{furnitureCode}", 
            async (string furnitureCode, IMediator mediator, CancellationToken cancellationToken) =>
        {
            await mediator.Send(new DeleteCommand(furnitureCode), cancellationToken);
            
            return Results.NoContent();
        });

        furnitureModelsGroup.MapPost("/{furnitureCode}/parts", 
            async (string furnitureCode, AddPartDefinitionRequest request,  IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new AddPartDefinitionCommand(
                furnitureCode,
                request.Name,
                request.LengthMm,
                request.WidthMm,
                request.ThicknessMm,
                request.Color,
                request.AdditionalInfo);
            
            var response = await mediator.Send(command, cancellationToken);
            
            return Results.Ok(response);
        });

        furnitureModelsGroup.MapPut("/{furnitureCode}/parts/{partDefinitionId}", 
            async (string furnitureCode, Guid partDefinitionId, UpdatePartDefinitionRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new UpdatePartDefinitionCommand(
                furnitureCode,
                partDefinitionId,
                request.Name,
                request.LengthMm,
                request.WidthMm,
                request.ThicknessMm,
                request.Color,
                request.AdditionalInfo);
            
            var response = await mediator.Send(command, cancellationToken);
            
            return Results.Ok(response);
        });

        furnitureModelsGroup.MapDelete("/{furnitureCode}/parts/{partDefinitionId}", 
            async (string furnitureCode, Guid partDefinitionId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new RemovePartDefinitionCommand(furnitureCode, partDefinitionId), cancellationToken);
            
            return Results.Ok(response);
        });
        
        return endpoints;
    }
    
    public static IRequestExecutorBuilder AddCatalogModuleGraphQlObjects(this IRequestExecutorBuilder builder)
    {
        return builder
            .RegisterDbContext<CatalogDataViewContext>()
            .AddType<CatalogViewResolver>();
    }
    
    private static IServiceCollection AddModuleDomainServices(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IAggregateRepository<FurnitureModel>), typeof(AggregateRepository<FurnitureModel, CatalogDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<FurnitureModel>), typeof(AggregateReadRepository<FurnitureModel, CatalogDbContext>));
    }
}
