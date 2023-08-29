using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Infrastructure.DomainServices;

public static class Extensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        // return services.AddScoped(typeof(IAggregateReadRepository<Test), typeof(AggregateReadRepository<Test, TestDbContext>))
        // return services.AddScoped(typeof(IAggregateRepository<Test), typeof(AggregateRepository<Test, TestDbContext>));
        
        return services;
    }
}