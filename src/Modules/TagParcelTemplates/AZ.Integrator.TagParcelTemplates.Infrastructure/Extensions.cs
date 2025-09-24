using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Repositories;
using AZ.Integrator.Stocks.Application.UseCases.AddStockGroup;
using AZ.Integrator.Stocks.Application.UseCases.ChangeQuantity;
using AZ.Integrator.Stocks.Application.UseCases.ChangeStockGroup;
using AZ.Integrator.Stocks.Application.UseCases.ChangeStockThreshold;
using AZ.Integrator.Stocks.Application.UseCases.RevertScanLog;
using AZ.Integrator.Stocks.Application.UseCases.UpdateStockGroup;
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
using Microsoft.AspNetCore.Mvc;
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
        endpoints.MapGet("/api/parcelTemplates/info", () => Results.Ok("Parcel Templates module")).AllowAnonymous();
        
        endpoints.MapPut("/api/parcelTemplates/{*tag}", async (string tag, SaveParcelTemplateCommand command, IMediator mediator, CancellationToken cancellationToken) =>
            {
                command = command with
                {
                    Tag = tag
                };
        
                await mediator.Send(command, cancellationToken);
                
                return Results.NoContent();
            })
            .RequireAuthorization();
        
        
        return endpoints;
    }
    
    public static IRequestExecutorBuilder AddTagParcelTemplatesModuleGraphQlObjects(this IRequestExecutorBuilder builder)
    {
        return builder
            .RegisterDbContext<TagParcelTemplateDataViewContext>()
            .AddType<TagParcelTemplatesViewResolver>();
    }
    
    private static IServiceCollection AddModuleDomainServices(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IAggregateRepository<TagParcelTemplate>), typeof(AggregateRepository<TagParcelTemplate, TagParcelTemplateDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<TagParcelTemplate>), typeof(AggregateReadRepository<TagParcelTemplate, TagParcelTemplateDbContext>));
    }
}