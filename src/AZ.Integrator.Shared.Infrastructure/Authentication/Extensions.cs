using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AZ.Integrator.Shared.Infrastructure.Authorization;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.ShipX;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AZ.Integrator.Shared.Infrastructure.Authentication;

internal static class Extensions
{
    private const string IdentityOptionsSectionName = "Infrastructure:Identity";
    private const string AllegroOptionsSectionName = "Infrastructure:Allegro";
    private const string ShipXOptionsSectionName = "Infrastructure:ShipX";
    
    public static IServiceCollection AddIntegratorAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityOptions>(configuration.GetRequiredSection(IdentityOptionsSectionName));
        var identityOptions = configuration.GetOptions<IdentityOptions>(IdentityOptionsSectionName);
        
        services.Configure<AllegroOptions>(configuration.GetRequiredSection(AllegroOptionsSectionName));
        var allegroOptions = configuration.GetOptions<AllegroOptions>(AllegroOptionsSectionName);
        
        services.Configure<ShipXOptions>(configuration.GetRequiredSection(ShipXOptionsSectionName));
        var shipXOptions = configuration.GetOptions<ShipXOptions>(ShipXOptionsSectionName);
            
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

        const string azTeamTenantCookieAuthenticationScheme = $"{CookieAuthenticationDefaults.AuthenticationScheme}-az-team";
        const string myTestTenantCookieAuthenticationScheme = $"{CookieAuthenticationDefaults.AuthenticationScheme}-my-test";
        
        const string azTeamTenantOAuthAuthenticationScheme = "allegro-az-team";
        const string myTestTenantOAuthAuthenticationScheme = "allegro-my-test";
        
        services
            .AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = azTeamTenantCookieAuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = azTeamTenantOAuthAuthenticationScheme;
            })
            .AddCookie(azTeamTenantCookieAuthenticationScheme)
            .AddCookie(myTestTenantCookieAuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
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
            })
            .AddOAuth(azTeamTenantOAuthAuthenticationScheme, options =>
            {
                options.ClientId = allegroOptions.AzTeamTenant.ClientId;
                options.ClientSecret = allegroOptions.AzTeamTenant.ClientSecret;
                options.CallbackPath = new PathString(allegroOptions.AzTeamTenant.RedirectUri);
                options.SignInScheme = azTeamTenantCookieAuthenticationScheme;
                
                ConfigureCommonOAuthOptions(options, allegroOptions, identityOptions, shipXOptions);
            })
            .AddOAuth(myTestTenantOAuthAuthenticationScheme, options =>
            {
                options.ClientId = allegroOptions.MyTestTenant.ClientId;
                options.ClientSecret = allegroOptions.MyTestTenant.ClientSecret;
                options.CallbackPath = new PathString(allegroOptions.MyTestTenant.RedirectUri);
                options.SignInScheme = myTestTenantCookieAuthenticationScheme;
                
                ConfigureCommonOAuthOptions(options, allegroOptions, identityOptions, shipXOptions);
            });
        
        return services;
    }

    private static void ConfigureCommonOAuthOptions(
        OAuthOptions options,
        AllegroOptions allegroOptions,
        IdentityOptions identityOptions,
        ShipXOptions shipXOptions)
    {
        options.AuthorizationEndpoint = allegroOptions.AuthorizationEndpoint;
        options.TokenEndpoint = allegroOptions.TokenEndpoint;

        options.SaveTokens = true;
                
        var innerHandler = new HttpClientHandler();
        options.BackchannelHttpHandler = new TokenExchangeAuthorizingHandler(innerHandler, options);

        options.Events.OnRedirectToAuthorizationEndpoint = ctx =>
        {
            ctx.RedirectUri = $"{ctx.RedirectUri}&prompt=confirm";

            ctx.HttpContext.Response.Redirect(ctx.RedirectUri);
            
            return Task.FromResult(0);
        };
                
        options.Events.OnCreatingTicket = ctx =>
        {
            var tenantId = ctx.Identity?.AuthenticationType;
                    
            var claims = new List<Claim>
            {
                new(UserClaimType.ShipXOrganizationId, shipXOptions.OrganizationId.ToString()),
                new(UserClaimType.TenantId, tenantId),
            };
                    
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: identityOptions.Issuer,
                audience: identityOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(identityOptions.ExpiresInHours),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identityOptions.PrivateKey)),
                    SecurityAlgorithms.HmacSha256)
            );

            ctx.Properties.StoreTokens(new[]
            {
                new AuthenticationToken
                {
                    Name = "integrator_access_token",
                    Value = jwtTokenHandler.WriteToken(jwtSecurityToken)
                }
            });
            return Task.CompletedTask;
        };
    }
}