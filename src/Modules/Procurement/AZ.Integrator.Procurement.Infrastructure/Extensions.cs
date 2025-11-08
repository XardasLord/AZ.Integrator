using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Procurement.Application;
using AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders;
using AZ.Integrator.Procurement.Contracts.PartDefinitionOrders;
using AZ.Integrator.Procurement.Contracts.Suppliers;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.DomainServices;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier;
using AZ.Integrator.Procurement.Infrastructure.DomainServices;
using AZ.Integrator.Procurement.Infrastructure.Persistence.EF;
using AZ.Integrator.Procurement.Infrastructure.Persistence.EF.Domain;
using AZ.Integrator.Procurement.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Procurement.Infrastructure.Persistence.GraphQL.QueryResolvers;
using AZ.Integrator.Shared.Infrastructure.Repositories;
using HotChocolate.Execution.Configuration;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Procurement.Infrastructure;

public static class Extensions
{
    public static IServiceCollection RegisterProcurementModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleApplication();
        services.AddModulePostgres(configuration);
        services.AddModuleDomainServices();

        return services;
    }
    
    public static IEndpointRouteBuilder MapProcurementModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string swaggerGroupName = "Procurement";
        
        MapSupplierEndpoints(endpoints, swaggerGroupName);
        MapOrderEndpoints(endpoints, swaggerGroupName);

        return endpoints;
    }

    private static void MapSupplierEndpoints(IEndpointRouteBuilder endpoints, string swaggerGroupName)
    {
        var suppliersGroup = endpoints.MapGroup("/api/procurement/suppliers").WithTags(swaggerGroupName).RequireAuthorization();
        
        suppliersGroup.MapGet("/info", () => Results.Ok("Suppliers module")).AllowAnonymous();
        
        suppliersGroup.MapPost("/", 
            async (CreateSupplierRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var command = new Application.UseCases.Suppliers.Commands.Create.CreateCommand(request.Name,
                        request.TelephoneNumber, request.Mailboxes);
                
                var response = await mediator.Send(command, cancellationToken);
            
                return Results.Ok(response);
            });
        
        suppliersGroup.MapPut("/{supplierId}", 
            async (int supplierId, UpdateSupplierRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var command = new Application.UseCases.Suppliers.Commands.Update.UpdateCommand(supplierId, request.Name,
                    request.TelephoneNumber, request.Mailboxes);
                
                var response = await mediator.Send(command, cancellationToken);
            
                return Results.Ok(response);
            });
    }
    
    private static void MapOrderEndpoints(IEndpointRouteBuilder endpoints, string swaggerGroupName)
    {
        var ordersGroup = endpoints.MapGroup("/api/procurement/orders").WithTags(swaggerGroupName).RequireAuthorization();
        
        ordersGroup.MapGet("/info", () => Results.Ok("Orders module")).AllowAnonymous();
        
        ordersGroup.MapPost("/", 
            async (CreateOrderRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var command = new Application.UseCases.PartDefinitionsOrders.Commands.Create.CreateCommand(
                    request.SupplierId, request.FurnitureLineRequests, request.AdditionalNotes);
                
                var response = await mediator.Send(command, cancellationToken);
            
                return Results.Ok(response);
            });
    }

    public static IRequestExecutorBuilder AddProcurementModuleGraphQlObjects(this IRequestExecutorBuilder builder)
    {
        return builder
            .RegisterDbContext<ProcurementDataViewContext>()
            .AddType<ProcurementViewResolver>();
    }
    
    private static IServiceCollection AddModuleDomainServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IOrderNumberGenerator, OrderNumberGenerator>()
            .AddTransient<IProcurementDataViewContext, ProcurementDataViewContext>()
            
            .AddScoped(typeof(IAggregateRepository<Supplier>), typeof(AggregateRepository<Supplier, ProcurementDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<Supplier>), typeof(AggregateReadRepository<Supplier, ProcurementDbContext>))
            
            .AddScoped(typeof(IAggregateRepository<PartDefinitionsOrder>), typeof(AggregateRepository<PartDefinitionsOrder, ProcurementDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<PartDefinitionsOrder>), typeof(AggregateReadRepository<PartDefinitionsOrder, ProcurementDbContext>));
    }
}
