using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Catalog.Application;

public static class Extensions
{
    public static IServiceCollection AddModuleApplication(this IServiceCollection services)
    {
        services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
}
