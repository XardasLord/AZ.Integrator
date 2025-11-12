using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Platform.FeatureFlags.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.Filters;

public sealed class RequireFeatureFlagFilter(string flagCode) : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var sp = context.HttpContext.RequestServices;
        var flags = sp.GetRequiredService<IFeatureFlags>();
        var currentUser = sp.GetRequiredService<ICurrentUser>();
        var tenantId = currentUser.TenantId;
        
        var enabled = await flags.IsEnabledAsync(flagCode, tenantId, context.HttpContext.RequestAborted);
        if (!enabled)
            return Results.Forbid();

        return await next(context);
    }
}