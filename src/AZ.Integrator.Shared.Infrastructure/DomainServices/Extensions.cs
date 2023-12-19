using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using AZ.Integrator.Shared.Infrastructure.Repositories;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.DomainServices;

public static class Extensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IAggregateRepository<InpostShipment>), typeof(AggregateRepository<InpostShipment, ShipmentDbContext>))
            .AddScoped(typeof(IAggregateRepository<DpdShipment>), typeof(AggregateRepository<DpdShipment, ShipmentDbContext>))
            .AddScoped(typeof(IAggregateRepository<Invoice>), typeof(AggregateRepository<Invoice, InvoiceDbContext>))
            .AddScoped(typeof(IAggregateRepository<TagParcelTemplate>), typeof(AggregateRepository<TagParcelTemplate, TagParcelTemplateDbContext>))
            
            .AddScoped(typeof(IAggregateReadRepository<InpostShipment>), typeof(AggregateReadRepository<InpostShipment, ShipmentDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<DpdShipment>), typeof(AggregateReadRepository<DpdShipment, ShipmentDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<Invoice>), typeof(AggregateReadRepository<Invoice, InvoiceDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<TagParcelTemplate>), typeof(AggregateReadRepository<TagParcelTemplate, TagParcelTemplateDbContext>));
    }
}