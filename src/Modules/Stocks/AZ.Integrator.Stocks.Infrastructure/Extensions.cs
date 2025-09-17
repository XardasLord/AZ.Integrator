using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Repositories;
using AZ.Integrator.Stocks.Application;
using AZ.Integrator.Stocks.Application.UseCases.AddStockGroup;
using AZ.Integrator.Stocks.Application.UseCases.ChangeQuantity;
using AZ.Integrator.Stocks.Application.UseCases.ChangeStockGroup;
using AZ.Integrator.Stocks.Application.UseCases.ChangeStockThreshold;
using AZ.Integrator.Stocks.Application.UseCases.RevertScanLog;
using AZ.Integrator.Stocks.Application.UseCases.UpdateStockGroup;
using AZ.Integrator.Stocks.Domain.Aggregates.Stock;
using AZ.Integrator.Stocks.Domain.Aggregates.StockGroup;
using AZ.Integrator.Stocks.Infrastructure.Persistence.EF;
using AZ.Integrator.Stocks.Infrastructure.Persistence.EF.Domain;
using AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Stocks.Infrastructure.Persistence.GraphQL.QueryResolvers;
using HotChocolate.Execution.Configuration;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Stocks.Infrastructure;

public static class Extensions
{
    public static IServiceCollection RegisterStocksModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleApplication(configuration);
        services.AddModulePostgres(configuration);
        services.AddModuleDomainServices();
        
        return services;
    }
    
    public static IEndpointRouteBuilder MapStocksModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/stocks/info", () => Results.Ok("Stocks module")).AllowAnonymous();
        
        endpoints.MapPut("/api/stocks/{*packageCode}", async (string packageCode, ChangeQuantityCommand command, IMediator mediator, CancellationToken cancellationToken) =>
            {
                command = command with
                {
                    PackageCode = packageCode
                };
                
                await mediator.Send(command, cancellationToken);
                
                return Results.NoContent();
            })
            .RequireAuthorization();
        
        endpoints.MapPut("/api/stocks/group/{*packageCode}", async (string packageCode, ChangeStockGroupCommand command, IMediator mediator, CancellationToken cancellationToken) =>
            {
                command = command with
                {
                    PackageCode = packageCode
                };
                
                await mediator.Send(command, cancellationToken);
                
                return Results.NoContent();
            })
            .RequireAuthorization();
        
        endpoints.MapPut("/api/stocks/threshold/{*packageCode}", async (string packageCode, ChangeStockThresholdCommand command, IMediator mediator, CancellationToken cancellationToken) =>
            {
                command = command with
                {
                    PackageCode = packageCode
                };
                
                await mediator.Send(command, cancellationToken);
                
                return Results.NoContent();
            })
            .RequireAuthorization();
        
        endpoints.MapDelete("/api/stock-logs/{scanLogId}", async (int scanLogId, [FromBody] RevertScanLogCommand command, IMediator mediator, CancellationToken cancellationToken) => 
            {
                command = command with
                {
                    ScanLogId = scanLogId
                };
                
                await mediator.Send(command, cancellationToken);
            
                return Results.NoContent();
            })
            .RequireAuthorization();
        
        endpoints.MapPost("/api/stock-groups", async (AddStockGroupCommand command, IMediator mediator, CancellationToken cancellationToken) => 
            {
                var stockGroupId = await mediator.Send(command, cancellationToken);

                return Results.Created($"/api/stock-groups/{stockGroupId}", new { Id = stockGroupId });
            })
            .RequireAuthorization();
        
        endpoints.MapPut("/api/stock-groups/{groupId}", async (int groupId, UpdateStockGroupCommand command, IMediator mediator, CancellationToken cancellationToken) => 
            {
                command = command with
                {
                    GroupId = groupId
                };
                
                await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .RequireAuthorization();
        
        return endpoints;
    }
    
    public static IRequestExecutorBuilder AddStocksModuleGraphQlObjects(this IRequestExecutorBuilder builder)
    {
        return builder
            .RegisterDbContext<StockDataViewContext>()
            .AddType<StocksViewResolver>();
    }
    
    private static IServiceCollection AddModuleDomainServices(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IAggregateRepository<Stock>), typeof(AggregateRepository<Stock, StockDbContext>))
            .AddScoped(typeof(IAggregateRepository<StockGroup>), typeof(AggregateRepository<StockGroup, StockDbContext>))
            
            .AddScoped(typeof(IAggregateReadRepository<Stock>), typeof(AggregateReadRepository<Stock, StockDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<StockGroup>), typeof(AggregateReadRepository<StockGroup, StockDbContext>));
    }
}