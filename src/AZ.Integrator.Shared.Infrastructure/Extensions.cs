using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Authentication;
using AZ.Integrator.Shared.Infrastructure.Authorization;
using AZ.Integrator.Shared.Infrastructure.DomainServices;
using AZ.Integrator.Shared.Infrastructure.ErrorHandling;
using AZ.Integrator.Shared.Infrastructure.ExternalServices;
using AZ.Integrator.Shared.Infrastructure.Hangfire;
using AZ.Integrator.Shared.Infrastructure.Hangfire.RecurringJobs;
using AZ.Integrator.Shared.Infrastructure.Identity;
using AZ.Integrator.Shared.Infrastructure.Mediator;
using AZ.Integrator.Shared.Infrastructure.OpenApi;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL;
using AZ.Integrator.Shared.Infrastructure.Time;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AZ.Integrator.Shared.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICurrentDateTime, CurrentDateTime>();
        
        services.AddHealthChecks();
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        services.AddCors();
        
        services.AddIntegratorAuthentication(configuration);
        services.AddIntegratorAuthorization(configuration);
        services.AddIntegratorIdentity(configuration);
            
        services.AddMediator(opt =>
        {
            opt.ServiceLifetime = ServiceLifetime.Scoped;
        });
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(HeaderRequestMiddleware<,>));
        
        services.AddPostgres(configuration);
        services.AddIntegratorGraphQl(configuration);
        services.AddIntegratorOpenApi(configuration);
        
        services.AddDomainServices();
        services.AddExternalServices(configuration);
        
        services.AddIntegratorJobManager(configuration)
            .AddRecurringJob<RefreshTenantAccessTokensRecurringJob>();
        
        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseIntegratorOpenApi();
        }
        
        app.UseCors(builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
        
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });

        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCookiePolicy(new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Lax
        });
        
        app.UseIntegratorHangfire();
        app.UseIntegratorGraphQl(configuration, env);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("api/healthz");
        });

        app.StartIntegratorRecurringJobs();
        
        return app;
    }
    
    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetRequiredSection(sectionName);
        section.Bind(options);

        return options;
    }
}