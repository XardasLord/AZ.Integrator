using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Platform.FeatureFlags.Abstractions;
using AZ.Integrator.Platform.FeatureFlags.Infrastructure.Entities;
using AZ.Integrator.Platform.Tenants.Infrastructure.Ef;
using AZ.Integrator.Platform.Tenants.Infrastructure.Entities;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Platform.Tenants.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddTenantsManagement(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PostgresOptions>(configuration.GetRequiredSection(PostgresOptions.SectionName));
        var postgresOptions = configuration.GetOptions<PostgresOptions>(PostgresOptions.SectionName);
        
        services.AddDbContext<TenantsDbContext>(options =>
        {
            options.EnableDetailedErrors();
            options.UseNpgsql(postgresOptions.ConnectionStringApplication);
        });
        
        return services;
    }

    public static IEndpointRouteBuilder MapTenantsManagementEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string swaggerGroupName = "Admin Tenants Management";
        
        var adminGroup = endpoints.MapGroup("/api/admin/tenants").WithTags(swaggerGroupName).RequireAuthorization();
        
        adminGroup.MapGet("/info", () => Results.Ok("Admin Tenants Management module")).AllowAnonymous();
        
        adminGroup.MapGet("/", 
            async ([FromServices] TenantsDbContext dbContext, [FromServices] ICurrentUser currentUser, CancellationToken cancellationToken) => 
            {
                if (!currentUser.IsMasterAdmin())
                    return Results.Forbid();
                
                var tenants = await dbContext.Tenants
                    .AsNoTracking()
                    .Include(x => x.FeatureFlags)
                    .ToListAsync(cancellationToken);
            
                return Results.Ok(tenants); 
            });
        
        adminGroup.MapPost("/", async (
            AddTenantRequest request,
            [FromServices] TenantsDbContext dbContext,
            [FromServices] ICurrentUser currentUser,
            [FromServices] ICurrentDateTime currentDateTime,
            CancellationToken cancellationToken) => 
            {
                if (!currentUser.IsMasterAdmin())
                    return Results.Forbid();

                var tenant = new Tenant
                {
                    TenantId = request.TenantId,
                    Name = request.Name,
                    IsActive = request.IsActive,
                    CreatedAt = currentDateTime.CurrentDate()
                };

                foreach (var featureFlag in FeatureFlagCodes.AllCodes)
                {
                    tenant.FeatureFlags.Add(new TenantFeatureFlag
                    {
                        Code = featureFlag,
                        Enabled = true,
                        ModifiedAt = currentDateTime.CurrentDate(),
                        ModifiedBy =  currentUser.UserId
                    });
                }
                
                dbContext.Tenants.Add(tenant);
                await dbContext.SaveChangesAsync(cancellationToken);
                
                // TODO: Create admin user for the tenant in Keycloak Identity Provider via API + attach user attributes as tenant_id and tenant_name
            
                return Results.Ok(); 
            });
        
        return endpoints;
    }

    public record AddTenantRequest(Guid TenantId, string Name, bool IsActive);
}