using System.Reflection;
using AZ.Integrator.Procurement.Application.Common.Settings;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Procurement.Application;

public static class Extensions
{
    public static IServiceCollection AddModuleApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .Configure<ApplicationSettings>(configuration.GetSection(ApplicationSettings.SectionName));
        
        return services;
    }
}
