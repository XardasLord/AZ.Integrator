using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Platform.FeatureFlags.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Filters;
using AZ.Integrator.Shared.Infrastructure.Repositories;
using AZ.Integrator.TagParcelTemplates.Application;
using AZ.Integrator.TagParcelTemplates.Application.UseCases.Commands.SaveParcelTemplate;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate;
using AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF;
using AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.Domain;
using AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.View;
using AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.GraphQL.QueryResolvers;
using HotChocolate.Execution.Configuration;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.TagParcelTemplates.Infrastructure;

public static class Extensions
{
    public static IServiceCollection RegisterTagParcelTemplatesModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleApplication(configuration);
        services.AddModulePostgres(configuration);
        services.AddModuleDomainServices();
        
        return services;
    }
    
    public static IEndpointRouteBuilder MapTagParcelTemplatesModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string swaggerGroupName = "Parcel templates";
        
        var parcelTemplates = endpoints.MapGroup("/api/parcelTemplates")
            .WithTags(swaggerGroupName)
            .RequireAuthorization();
        
        parcelTemplates.MapGet("/info", () => Results.Ok("Parcel Templates module")).AllowAnonymous();
        
        parcelTemplates.MapPut("/{*tag}", async (string tag, SaveParcelTemplateCommand command, IMediator mediator, CancellationToken cancellationToken) =>
        {
            command = command with
            {
                Tag = tag
            };
            
            await mediator.Send(command, cancellationToken);
            
            return Results.NoContent();
        })
        .RequireFeatureFlag(FeatureFlagCodes.ParcelTemplatesModule);
        
        return endpoints;
    }
    
    public static IRequestExecutorBuilder AddTagParcelTemplatesModuleGraphQlObjects(this IRequestExecutorBuilder builder)
    {
        return builder
            .RegisterDbContextFactory<TagParcelTemplateDataViewContext>()
            .AddType<TagParcelTemplatesViewResolver>();
    }
    
    private static IServiceCollection AddModuleDomainServices(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IAggregateRepository<TagParcelTemplate>), typeof(AggregateRepository<TagParcelTemplate, TagParcelTemplateDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<TagParcelTemplate>), typeof(AggregateReadRepository<TagParcelTemplate, TagParcelTemplateDbContext>));
    }
}