﻿using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Invoices.Application;

public static class Extensions
{
    public static IServiceCollection AddInvoicesModuleApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR(typeof(Extensions).Assembly)
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
}