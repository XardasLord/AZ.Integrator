using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Repositories;
using AZ.Integrator.Stocks.Application;
using AZ.Integrator.Stocks.Application.UseCases.AddStockGroup;
using AZ.Integrator.Stocks.Application.UseCases.ChangeQuantity;
using AZ.Integrator.Stocks.Application.UseCases.RevertScanLog;
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
    
    public static IEndpointRouteBuilder MapStocksEndpoints(this IEndpointRouteBuilder endpoints)
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
        
        endpoints.MapPost("/api/stocks-groups", async (AddStockGroupCommand command, IMediator mediator, CancellationToken cancellationToken) => 
            {
                var stockGroupId = await mediator.Send(command, cancellationToken);

                return Results.Created($"/api/stocks-groups/{stockGroupId}", new { Id = stockGroupId });
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