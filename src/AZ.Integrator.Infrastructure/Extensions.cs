using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Infrastructure.Authentication;
using AZ.Integrator.Infrastructure.Authorization;
using AZ.Integrator.Infrastructure.DomainServices;
using AZ.Integrator.Infrastructure.ErrorHandling;
using AZ.Integrator.Infrastructure.ExternalServices;
using AZ.Integrator.Infrastructure.Hangfire;
using AZ.Integrator.Infrastructure.Identity;
using AZ.Integrator.Infrastructure.OpenApi;
using AZ.Integrator.Infrastructure.Persistence.EF;
using AZ.Integrator.Infrastructure.Persistence.GraphQL;
using AZ.Integrator.Infrastructure.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AZ.Integrator.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICurrentDateTime, CurrentDateTime>();
        
        services.AddHealthChecks();
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        
        services.AddIntegratorAuthentication(configuration);
        services.AddIntegratorAuthorization(configuration);
        services.AddIntegratorIdentity(configuration);
        
        services.AddPostgres(configuration);
        services.AddIntegratorHangfire(configuration);
        services.AddIntegratorGraphQl(configuration);
        services.AddIntegratorOpenApi(configuration);
        
        services.AddDomainServices();
        services.AddExternalServices(configuration);

        services.AddCors();
        
        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseIntegratorOpenApi();
        }
        else
        {
            // app.UseHttpsRedirection();
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
            Secure = CookieSecurePolicy.Always
        });
        
        app.UseIntegratorHangfire();
        app.UseIntegratorGraphQl(configuration, env);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("api/healthz");
        });
        
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