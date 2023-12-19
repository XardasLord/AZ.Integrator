using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.TagParcelTemplates.Application;

public static class Extensions
{
    public static IServiceCollection AddTagParcelTemplatesModuleApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR(typeof(Extensions).Assembly)
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
}