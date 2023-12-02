using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AZ.Integrator.Shared.Infrastructure.Authorization;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro;
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
    
    public static IServiceCollection AddIntegratorAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityOptions>(configuration.GetRequiredSection(IdentityOptionsSectionName));
        var identityOptions = configuration.GetOptions<IdentityOptions>(IdentityOptionsSectionName);
        
        services.Configure<AllegroOptions>(configuration.GetRequiredSection(AllegroOptionsSectionName));
        var allegroOptions = configuration.GetOptions<AllegroOptions>(AllegroOptionsSectionName);
            
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

        var azTeamTenantCookieAuthenticationScheme = $"{CookieAuthenticationDefaults.AuthenticationScheme}-az-team";
        var myTestTenantCookieAuthenticationScheme = $"{CookieAuthenticationDefaults.AuthenticationScheme}-my-test";
        
        var azTeamTenantOAuthAuthenticationScheme = "allegro-az-team";
        var myTestTenantOAuthAuthenticationScheme = "allegro-my-test";
        
        services
            .AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = azTeamTenantCookieAuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = azTeamTenantOAuthAuthenticationScheme;
            })
            .AddCookie(azTeamTenantCookieAuthenticationScheme, options =>
            {
                // add an instance of the patched manager to the options:
                // options.CookieManager = new ChunkingCookieManager();
                //
                // options.Cookie.HttpOnly = true;
                // options.Cookie.SameSite = SameSiteMode.None;
                // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddCookie(myTestTenantCookieAuthenticationScheme, options =>
            {
                // add an instance of the patched manager to the options:
                // options.CookieManager = new ChunkingCookieManager();
                //
                // options.Cookie.HttpOnly = true;
                // options.Cookie.SameSite = SameSiteMode.None;
                // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
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
                
                ConfigureCommonOAuthOptions(configuration, options, allegroOptions, identityOptions);
            })
            .AddOAuth(myTestTenantOAuthAuthenticationScheme, options =>
            {
                options.ClientId = allegroOptions.MyTestTenant.ClientId;
                options.ClientSecret = allegroOptions.MyTestTenant.ClientSecret;
                options.CallbackPath = new PathString(allegroOptions.MyTestTenant.RedirectUri);
                options.SignInScheme = myTestTenantCookieAuthenticationScheme;
                
                ConfigureCommonOAuthOptions(configuration, options, allegroOptions, identityOptions);
            });
        
        return services;
    }

    private static void ConfigureCommonOAuthOptions(
        IConfiguration configuration,
        OAuthOptions options,
        AllegroOptions allegroOptions,
        IdentityOptions identityOptions)
    {
        options.AuthorizationEndpoint = allegroOptions.AuthorizationEndpoint;
        options.TokenEndpoint = allegroOptions.TokenEndpoint;

        options.SaveTokens = true;
                
        // options.CorrelationCookie.HttpOnly = true;
        // options.CorrelationCookie.SameSite = SameSiteMode.Lax;
                
        var innerHandler = new HttpClientHandler();
        options.BackchannelHttpHandler = new TokenExchangeAuthorizingHandler(innerHandler, options);

        options.Events.OnRedirectToAuthorizationEndpoint = ctx =>
        {
            ctx.RedirectUri = $"{ctx.RedirectUri}&prompt=confirm&state=";

            ctx.HttpContext.Response.Redirect(ctx.RedirectUri);
            
            return Task.FromResult(0);
        };

        options.Events.OnAccessDenied = ctx =>
        {
            ctx.HttpContext.Response.Redirect(ctx.ReturnUrl);

            return Task.CompletedTask;
        };

        options.Events.OnRemoteFailure = ctx =>
        {
            // ctx.HttpContext.Response.Redirect;

            return Task.CompletedTask;
        };
                
        options.Events.OnCreatingTicket = ctx =>
        {
            var allegroAccessToken = ctx.AccessToken;
            // var allegroRefreshToken = ctx.RefreshToken;
            var tenantId = ctx.Identity?.AuthenticationType;
                    
            var claims = new List<Claim>
            {
                new(UserClaimType.AllegroAccessToken, allegroAccessToken),
                // new(UserClaimType.AllegroRefreshToken, allegroRefreshToken),
                new(UserClaimType.ShipXOrganizationId, configuration["Infrastructure:ShipX:OrganizationId"]),
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