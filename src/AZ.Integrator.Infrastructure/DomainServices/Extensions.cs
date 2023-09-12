using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.Aggregates.Order;
using AZ.Integrator.Infrastructure.Persistence.EF.DbContexts;
using AZ.Integrator.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Infrastructure.DomainServices;

public static class Extensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IAggregateReadRepository<Order>), typeof(AggregateReadRepository<Order, OrderDbContext>))
            .AddScoped(typeof(IAggregateRepository<Order>), typeof(AggregateRepository<Order, OrderDbContext>));
    }
}