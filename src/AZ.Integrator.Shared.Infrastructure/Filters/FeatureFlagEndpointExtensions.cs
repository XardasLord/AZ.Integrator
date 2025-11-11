using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AZ.Integrator.Shared.Infrastructure.Filters;

public static class FeatureFlagEndpointExtensions
{
    public static RouteHandlerBuilder RequireFeatureFlag(this RouteHandlerBuilder builder, string flagCode)
    {
        builder.AddEndpointFilter(new RequireFeatureFlagFilter(flagCode));
        
        return builder;
    }
}