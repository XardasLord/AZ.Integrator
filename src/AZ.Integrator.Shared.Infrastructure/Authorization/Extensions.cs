using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shared.Infrastructure.UserInfo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.Authorization;

internal static class Extensions
{
    public static IServiceCollection AddIntegratorAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddScoped<ICurrentUser, CurrentUser>();
        // .AddAuthorization(options => options.AddSrmsPolicies());
    }
}