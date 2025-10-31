using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Procurement.Application;
using AZ.Integrator.Procurement.Application.UseCases.Suppliers.Commands.Create;
using AZ.Integrator.Procurement.Application.UseCases.Suppliers.Commands.Update;
using AZ.Integrator.Procurement.Contracts.Suppliers;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier;
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
        
        var suppliersGroup = endpoints.MapGroup("/api/procurement/suppliers").WithTags(swaggerGroupName).RequireAuthorization();
        
        suppliersGroup.MapGet("/info", () => Results.Ok("Suppliers module")).AllowAnonymous();
        
        suppliersGroup.MapPost("/", 
            async (CreateSupplierRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(new CreateCommand(request.Name, request.TelephoneNumber, request.Mailboxes), cancellationToken);
            
                return Results.Ok(response);
            });
        
        suppliersGroup.MapPut("/{supplierId}", 
            async (int supplierId, UpdateSupplierRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(new UpdateCommand(supplierId, request.Name, request.TelephoneNumber, request.Mailboxes), cancellationToken);
            
                return Results.Ok(response);
            });
        
        return endpoints;
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
            .AddScoped(typeof(IAggregateRepository<Supplier>), typeof(AggregateRepository<Supplier, ProcurementDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<Supplier>), typeof(AggregateReadRepository<Supplier, ProcurementDbContext>));
    }
}
