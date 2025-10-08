using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.Extensions;
using AZ.Integrator.Invoices.Application;
using AZ.Integrator.Invoices.Application.UseCases.Invoices.Queries.Download;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice;
using AZ.Integrator.Invoices.Infrastructure.ExternalServices.Fakturownia;
using AZ.Integrator.Invoices.Infrastructure.Persistence.EF;
using AZ.Integrator.Invoices.Infrastructure.Persistence.EF.Domain;
using AZ.Integrator.Invoices.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Invoices.Infrastructure.Persistence.GraphQL.QueryResolvers;
using AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder;
using AZ.Integrator.Shared.Infrastructure.Repositories;
using HotChocolate.Execution.Configuration;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Invoices.Infrastructure;

public static class Extensions
{
    public static IServiceCollection RegisterInvoicesModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleApplication(configuration);
        services.AddModulePostgres(configuration);
        services.AddModuleDomainServices();
        
        services.AddFakturownia(configuration);
        
        return services;
    }
    
    public static IEndpointRouteBuilder MapInvoicesModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string swaggerGroupName = "Invoices";
        
        var invoiceGroup = endpoints.MapGroup("/api/invoices").WithTags(swaggerGroupName).RequireAuthorization();
        
        invoiceGroup.MapGet("/info", () => Results.Ok("Invoices module")).AllowAnonymous();
        
        invoiceGroup.MapPost("/", async (GenerateInvoiceForOrderCommand command, IMediator mediator, CancellationToken cancellationToken) =>
        {
            command = command with
            {
                CorrelationId = command.CorrelationId ?? CorrelationIdHelper.New()
            };
            
            await mediator.Send(command, cancellationToken);
            
            return Results.Ok();
        });
        
        invoiceGroup.MapGet("/{invoiceId}", async (long invoiceId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new DownloadQuery(invoiceId), cancellationToken);
            
            return Results.File(result.ContentStream, result.ContentType, result.FileName);
        });
        
        return endpoints;
    }
    
    public static IRequestExecutorBuilder AddInvoicesModuleGraphQlObjects(this IRequestExecutorBuilder builder)
    {
        return builder
            .RegisterDbContext<InvoiceDataViewContext>()
            .AddType<InvoicesViewResolver>();
    }
    
    private static IServiceCollection AddModuleDomainServices(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IAggregateRepository<Invoice>), typeof(AggregateRepository<Invoice, InvoiceDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<Invoice>), typeof(AggregateReadRepository<Invoice, InvoiceDbContext>));
    }
}