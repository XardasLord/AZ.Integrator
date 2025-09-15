using AZ.Integrator.Orders.Application;
using AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetAll;
using AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetDetails;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Orders.Infrastructure;

public static class Extensions
{
    public static IServiceCollection RegisterOrdersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleApplication(configuration);
        
        return services;
    }
    
    public static IEndpointRouteBuilder MapOrdersModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/orders/info", () => Results.Ok("Orders module")).AllowAnonymous();
        
        endpoints.MapGet("/api/orders", async ([AsParameters] GetAllQueryFilters filters, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var orders = await mediator.Send(new GetAllQuery(filters), cancellationToken);
                
                return Results.Ok(orders);
            })
            .RequireAuthorization();
        
        endpoints.MapPut("/api/orders/{orderId}", async (string orderId, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var order = await mediator.Send(new GetDetailsQuery(orderId), cancellationToken);
                
                return Results.Ok(order);
            })
            .RequireAuthorization();
        
        return endpoints;
    }
}