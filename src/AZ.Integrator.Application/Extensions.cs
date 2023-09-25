using System.Reflection;
using AZ.Integrator.Application.Common.Mediator.Pipeline;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediator(opt => opt.ServiceLifetime = ServiceLifetime.Scoped)
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddSingleton(typeof(IPipelineBehavior<,>), typeof(ErrorLoggingBehaviour<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(MessageValidatorBehaviour<,>));
        
        return services;
    }
}