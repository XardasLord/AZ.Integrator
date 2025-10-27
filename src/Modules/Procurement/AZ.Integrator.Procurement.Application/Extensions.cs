using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Procurement.Application;

public static class Extensions
{
    public static IServiceCollection AddModuleApplication(this IServiceCollection services)
    {
        services
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
}
