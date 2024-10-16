using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Orders.Application;

public static class Extensions
{
    public static IServiceCollection AddOrdersModuleApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
}