using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.Identity;

public static class Extensions
{
    public static IServiceCollection AddIntegratorIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentityCore<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<UserDbContext>();

        services.AddScoped<TokenService>();

        return services;
    }
}