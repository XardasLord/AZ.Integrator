using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.Dpd;
using AZ.Integrator.Shared.Infrastructure.Repositories;
using AZ.Integrator.Shipments.Application;
using AZ.Integrator.Shipments.Application.UseCases.Shipments.Commands.CreateDpdShipment;
using AZ.Integrator.Shipments.Application.UseCases.Shipments.Commands.CreateInpostShipment;
using AZ.Integrator.Shipments.Application.UseCases.Shipments.Queries.GetDpdLabel;
using AZ.Integrator.Shipments.Application.UseCases.Shipments.Queries.GetInpostLabel;
using AZ.Integrator.Shipments.Application.UseCases.Shipments.Queries.GetInpostLabels;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;
using AZ.Integrator.Shipments.Infrastructure.ExternalServices.ShipX;
using AZ.Integrator.Shipments.Infrastructure.Persistence;
using AZ.Integrator.Shipments.Infrastructure.Persistence.EF.Domain;
using AZ.Integrator.Shipments.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Shipments.Infrastructure.Persistence.GraphQL.QueryResolvers;
using HotChocolate.Execution.Configuration;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shipments.Infrastructure;

public static class Extensions
{
    public static IServiceCollection RegisterShipmentsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleApplication(configuration);
        services.AddModulePostgres(configuration);
        services.AddModuleDomainServices();

        services.AddDpd(configuration);
        services.AddShipX(configuration);
        
        return services;
    }
    
    public static IEndpointRouteBuilder MapShipmentsModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string swaggerGroupNameInpost = "Inpost Shipments";
        
        var inpostShipmentGroup = endpoints.MapGroup("/api/inpostShipments").WithTags(swaggerGroupNameInpost).RequireAuthorization();
        
        inpostShipmentGroup.MapGet("/info", () => Results.Ok("Inpost Shipments module")).AllowAnonymous();
        
        inpostShipmentGroup.MapPost("/", async (CreateInpostShipmentCommand command, IMediator mediator, CancellationToken cancellationToken) =>
        {
            await mediator.Send(command, cancellationToken);
            
            return Results.Ok();
        });
        
        inpostShipmentGroup.MapGet("/{shipmentId}/label", async (string shipmentId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetInpostLabelQuery(shipmentId), cancellationToken);
            
            return Results.File(result.ContentStream, result.ContentType, result.FileName);
        });
        
        inpostShipmentGroup.MapGet("/label", async ([FromQuery] string[] shipmentNumber, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetInpostLabelsQuery(shipmentNumber), cancellationToken);
            
            return Results.File(result.ContentStream, result.ContentType, result.FileName);
        });
        
        
        const string swaggerGroupNameDpd = "Dpd Shipments";
        
        var dpdShipmentGroup = endpoints.MapGroup("/api/dpdShipments").WithTags(swaggerGroupNameDpd).RequireAuthorization();
        
        dpdShipmentGroup.MapGet("/info", () => Results.Ok("Dpd Shipments module")).AllowAnonymous();

        dpdShipmentGroup.MapPost("/", async (CreateDpdShipmentCommand command, IMediator mediator, CancellationToken cancellationToken) =>
        {
            await mediator.Send(command, cancellationToken);
            
            return Results.Ok();
        });
        
        dpdShipmentGroup.MapGet("/{sessionId}/label", async (long sessionId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetDpdLabelQuery(sessionId), cancellationToken);
            
            return Results.File(result.ContentStream, result.ContentType, result.FileName);
        });
        
        return endpoints;
    }
    
    public static IRequestExecutorBuilder AddShipmentsModuleGraphQlObjects(this IRequestExecutorBuilder builder)
    {
        return builder
            .RegisterDbContext<ShipmentDataViewContext>()
            .AddType<ShipmentsViewResolver>();
    }
    
    private static IServiceCollection AddModuleDomainServices(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IAggregateRepository<InpostShipment>), typeof(AggregateRepository<InpostShipment, ShipmentDbContext>))
            .AddScoped(typeof(IAggregateRepository<DpdShipment>), typeof(AggregateRepository<DpdShipment, ShipmentDbContext>))
            
            .AddScoped(typeof(IAggregateReadRepository<InpostShipment>), typeof(AggregateReadRepository<InpostShipment, ShipmentDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<DpdShipment>), typeof(AggregateReadRepository<DpdShipment, ShipmentDbContext>));
    }
    
    private class GetInpostLabelsRequest
    {
        public IEnumerable<string> ShipmentNumber { get; set; } = [];
    }
}