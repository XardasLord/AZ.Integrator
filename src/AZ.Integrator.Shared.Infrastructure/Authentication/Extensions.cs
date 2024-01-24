using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AZ.Integrator.Shared.Infrastructure.Authorization;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.ShipX;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
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

        var clientUrlAppRedirect = configuration["Application:ClientAppUrl"];
            
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

        const string azTeamTenantCookieAuthenticationScheme = $"{CookieAuthenticationDefaults.AuthenticationScheme}-az-team";
        const string mebleplTenantCookieAuthenticationScheme = $"{CookieAuthenticationDefaults.AuthenticationScheme}-meblepl";
        const string myTestTenantCookieAuthenticationScheme = $"{CookieAuthenticationDefaults.AuthenticationScheme}-my-test";
        
        const string azTeamTenantOAuthAuthenticationScheme = "allegro-az-team";
        const string mebleplTenantOAuthAuthenticationScheme = "allegro-meblepl";
        const string myTestTenantOAuthAuthenticationScheme = "allegro-my-test";
        
        services
            .AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = azTeamTenantCookieAuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = azTeamTenantOAuthAuthenticationScheme;
            })
            .AddCookie(azTeamTenantCookieAuthenticationScheme)
            .AddCookie(mebleplTenantCookieAuthenticationScheme)
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
                
                ConfigureCommonOAuthOptions(services, options, allegroOptions, identityOptions, shipXOptions, clientUrlAppRedirect);
            })
            .AddOAuth(myTestTenantOAuthAuthenticationScheme, options =>
            {
                options.ClientId = allegroOptions.MyTestTenant.ClientId;
                options.ClientSecret = allegroOptions.MyTestTenant.ClientSecret;
                options.CallbackPath = new PathString(allegroOptions.MyTestTenant.RedirectUri);
                options.SignInScheme = myTestTenantCookieAuthenticationScheme;
                
                ConfigureCommonOAuthOptions(services, options, allegroOptions, identityOptions, shipXOptions, clientUrlAppRedirect);
            })
            .AddOAuth(mebleplTenantOAuthAuthenticationScheme, options =>
            {
                options.ClientId = allegroOptions.MebleplTenant.ClientId;
                options.ClientSecret = allegroOptions.MebleplTenant.ClientSecret;
                options.CallbackPath = new PathString(allegroOptions.MebleplTenant.RedirectUri);
                options.SignInScheme = mebleplTenantCookieAuthenticationScheme;
                
                ConfigureCommonOAuthOptions(services, options, allegroOptions, identityOptions, shipXOptions, clientUrlAppRedirect);
            });
        
        return services;
    }

    private static void ConfigureCommonOAuthOptions(
        IServiceCollection services,
        OAuthOptions options,
        AllegroOptions allegroOptions,
        IdentityOptions identityOptions,
        ShipXOptions shipXOptions,
        string clientUrlAppRedirect)
    {
        options.AuthorizationEndpoint = allegroOptions.AuthorizationEndpoint;
        options.TokenEndpoint = allegroOptions.TokenEndpoint;
        
        options.SaveTokens = true;

        var innerHandler = new HttpClientHandler();
        options.BackchannelHttpHandler = new TokenExchangeAuthorizingHandler(innerHandler, options);

        options.Events.OnRedirectToAuthorizationEndpoint = ctx =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var dbViewContext = serviceProvider.GetRequiredService<AllegroAccountDataViewContext>();

            var tenantId = $"allegro-{ctx.Request.Query["tenantId"].ToString()}";

            var allegroAccount = dbViewContext.AllegroAccounts.SingleOrDefault(x => x.TenantId == tenantId);
            if (allegroAccount is null)
            {
                // Account does not exist in the system
                ctx.RedirectUri = $"{ctx.RedirectUri}&prompt=confirm";

                ctx.HttpContext.Response.Redirect(ctx.RedirectUri);
            
                return Task.FromResult(0);
            }
            else
            {
                // Account exists, so we don't need to force login
                var jwtToken = GenerateJwtToken(tenantId, identityOptions, shipXOptions);
                
                ctx.Properties.StoreTokens(new[]
                {
                    new AuthenticationToken
                    {
                        Name = "integrator_access_token",
                        Value = jwtToken
                    }
                });
                
                ctx.HttpContext.Response.Redirect($"{clientUrlAppRedirect}?access_token={jwtToken}");
                
                return Task.CompletedTask;
            }
        };

        options.Events.OnCreatingTicket = ctx =>
        {
            // TODO: Here comes a new account that does not exist in DB in the system, so at the end of this event we need to create a new tenant account in the system
            var tenantId = ctx.Identity?.AuthenticationType;
                    
            var jwtToken = GenerateJwtToken(tenantId, identityOptions, shipXOptions);

            ctx.Properties.StoreTokens(new[]
            {
                new AuthenticationToken
                {
                    Name = "integrator_access_token",
                    Value = jwtToken
                }
            });
            
            // TODO: We should save new account in DB
            
            return Task.CompletedTask;
        };
    }

    private static string GenerateJwtToken(string tenantId, IdentityOptions identityOptions, ShipXOptions shipXOptions)
    {
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

        return jwtTokenHandler.WriteToken(jwtSecurityToken);
    }
}