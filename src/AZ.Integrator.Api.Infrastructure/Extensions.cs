using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Invoices.Infrastructure;
using AZ.Integrator.Orders.Infrastructure;
using AZ.Integrator.TagParcelTemplates.Infrastructure;
using AZ.Integrator.Shared.Infrastructure.Authentication;
using AZ.Integrator.Shared.Infrastructure.Authorization;
using AZ.Integrator.Shared.Infrastructure.ErrorHandling;
using AZ.Integrator.Shared.Infrastructure.Hangfire;
using AZ.Integrator.Shared.Infrastructure.Hangfire.RecurringJobs;
using AZ.Integrator.Shared.Infrastructure.Identity;
using AZ.Integrator.Shared.Infrastructure.Mediator;
using AZ.Integrator.Shared.Infrastructure.OpenApi;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL;
using AZ.Integrator.Shared.Infrastructure.Time;
using AZ.Integrator.Shipments.Infrastructure;
using AZ.Integrator.Stocks.Infrastructure;
using AZ.Integrator.Monitoring.Infrastructure;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AZ.Integrator.Api.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddApiInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks();
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        services.AddCors();
        
        services.AddScoped<ICurrentDateTime, CurrentDateTime>();
        
        services.AddIntegratorAuthentication(configuration);
        services.AddIntegratorAuthorization(configuration);
        services.AddIntegratorIdentity(configuration);
        
        services.AddMediator(opt =>
        {
            opt.ServiceLifetime = ServiceLifetime.Scoped;
        });
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(HeaderRequestMiddleware<,>));
        
        services.AddPostgres(configuration);
        services.AddIntegratorGraphQl(configuration)
            .AddStocksModuleGraphQlObjects()
            .AddTagParcelTemplatesModuleGraphQlObjects()
            .AddInvoicesModuleGraphQlObjects()
            .AddShipmentsModuleGraphQlObjects()
            .AddMonitoringModuleGraphQlObjects();
        
        services.AddIntegratorOpenApi(configuration);
        
        services.AddIntegratorJobManager(configuration)
            .AddRecurringJob<RefreshTenantAccessTokensRecurringJob>();
        
        // Infrastructure dedicated modules
        services.RegisterStocksModule(configuration);
        services.RegisterOrdersModule(configuration);
        services.RegisterTagParcelTemplatesModule(configuration);
        services.RegisterInvoicesModule(configuration);
        services.RegisterShipmentsModule(configuration);
        services.RegisterMonitoringModule(configuration);

        return services;
    }
    
    public static IApplicationBuilder UseApiInfrastructure(this IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment env)
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
}