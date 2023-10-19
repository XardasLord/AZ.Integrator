using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.Aggregates.DpdShipment;
using AZ.Integrator.Domain.Aggregates.InpostShipment;
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
            .AddScoped(typeof(IAggregateRepository<DpdShipment>), typeof(AggregateRepository<DpdShipment, ShipmentDbContext>));
    }
}