using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AZ.Integrator.Infrastructure.Authentication;

internal static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:Identity";
    
    public static IServiceCollection AddIntegratorAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityOptions>(configuration.GetRequiredSection(OptionsSectionName));
        
        var identityOptions = configuration.GetOptions<IdentityOptions>(OptionsSectionName);
            
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
        
        services
            .AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // options.Authority = identityOptions.Authority;
                // options.MetadataAddress = identityOptions.MetadataAddress;
                // options.Audience = identityOptions.ClientId;
                options.RequireHttpsMetadata = false;
                options.IncludeErrorDetails = true;
        
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromSeconds(5),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(identityOptions.PrivateKey)
                    ),
                    ValidIssuer = identityOptions.Issuer,
                    ValidAudience = identityOptions.Audience
                };
            });
        
        return services;
    }
}