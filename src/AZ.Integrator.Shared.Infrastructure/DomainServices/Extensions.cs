using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain.Shipment;
using AZ.Integrator.Shared.Infrastructure.Repositories;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.DomainServices;

public static class Extensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IAggregateRepository<InpostShipment>), typeof(AggregateRepository<InpostShipment, ShipmentDbContext>))
            .AddScoped(typeof(IAggregateRepository<DpdShipment>), typeof(AggregateRepository<DpdShipment, ShipmentDbContext>))
            
            .AddScoped(typeof(IAggregateReadRepository<InpostShipment>), typeof(AggregateReadRepository<InpostShipment, ShipmentDbContext>))
            .AddScoped(typeof(IAggregateReadRepository<DpdShipment>), typeof(AggregateReadRepository<DpdShipment, ShipmentDbContext>));
    }
}