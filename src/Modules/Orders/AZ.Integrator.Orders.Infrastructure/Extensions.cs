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
        const string swaggerGroupName = "Orders";
        
        var ordersGroup = endpoints.MapGroup("/api/orders").WithTags(swaggerGroupName).RequireAuthorization();
        
        ordersGroup.MapGet("/info", () => Results.Ok("Orders module")).AllowAnonymous();
        
        ordersGroup.MapGet("/", async ([AsParameters] GetAllQueryFilters filters, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var orders = await mediator.Send(new GetAllQuery(filters), cancellationToken);
                
            return Results.Ok(orders);
        });
        
        ordersGroup.MapPut("/{orderId}", async (string orderId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var order = await mediator.Send(new GetDetailsQuery(orderId), cancellationToken);
                
            return Results.Ok(order);
        });
        
        return endpoints;
    }
}