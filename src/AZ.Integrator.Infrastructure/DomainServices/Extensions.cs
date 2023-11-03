using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.Aggregates.DpdShipment;
using AZ.Integrator.Domain.Aggregates.InpostShipment;
using AZ.Integrator.Domain.Aggregates.Invoice;
using AZ.Integrator.Infrastructure.Persistence.EF.DbContexts;
using AZ.Integrator.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Infrastructure.DomainServices;

public static class Extensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IAggregateRepository<InpostShipment>), typeof(AggregateRepository<InpostShipment, ShipmentDbContext>))
            .AddScoped(typeof(IAggregateRepository<DpdShipment>), typeof(AggregateRepository<DpdShipment, ShipmentDbContext>))
            .AddScoped(typeof(IAggregateRepository<Invoice>), typeof(AggregateRepository<Invoice, InvoiceDbContext>))
            
            .AddScoped(typeof(IAggregateReadRepository<InpostShipment>), typeof(AggregateReadRepository<InpostShipment, ShipmentDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<DpdShipment>), typeof(AggregateReadRepository<DpdShipment, ShipmentDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<Invoice>), typeof(AggregateReadRepository<Invoice, InvoiceDbContext>));
    }
}