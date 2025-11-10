using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Stocks.Application;

public static class Extensions
{
    public static IServiceCollection AddModuleApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
}