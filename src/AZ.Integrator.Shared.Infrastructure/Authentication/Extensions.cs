using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.AllegroAccount;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AZ.Integrator.Shared.Infrastructure.Authentication;

public static class Extensions
{
    private const string IdentityOptionsSectionName = "Infrastructure:Identity";
    private const string AllegroOptionsSectionName = "Infrastructure:Allegro";
    
    public static IServiceCollection AddIntegratorAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityOptions>(configuration.GetRequiredSection(IdentityOptionsSectionName));
        var identityOptions = configuration.GetOptions<IdentityOptions>(IdentityOptionsSectionName);
        
        services.Configure<AllegroOptions>(configuration.GetRequiredSection(AllegroOptionsSectionName));
        var allegroOptions = configuration.GetOptions<AllegroOptions>(AllegroOptionsSectionName);

        var clientUrlAppRedirect = configuration["Application:ClientAppUrl"];
            
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

        const string allegroAzTeamTenantCookieAuthenticationScheme = $"{CookieAuthenticationDefaults.AuthenticationScheme}-az-team";
        const string allegroMebleplTenantCookieAuthenticationScheme = $"{CookieAuthenticationDefaults.AuthenticationScheme}-meblepl";
        const string allegroMyTestTenantCookieAuthenticationScheme = $"{CookieAuthenticationDefaults.AuthenticationScheme}-my-test";
        
        const string allegroAzTeamTenantOAuthAuthenticationScheme = "allegro-az-team";
        const string allegroMebleplTenantOAuthAuthenticationScheme = "allegro-meblepl";
        const string allegroMyTestTenantOAuthAuthenticationScheme = "allegro-my-test";
        
        services
            .AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                // sharedOptions.DefaultChallengeScheme = allegroAzTeamTenantOAuthAuthenticationScheme;
            })
            .AddCookie(allegroAzTeamTenantCookieAuthenticationScheme)
            .AddCookie(allegroMebleplTenantCookieAuthenticationScheme)
            .AddCookie(allegroMyTestTenantCookieAuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                // options.Authority = identityOptions.Authority;
                // options.MetadataAddress = identityOptions.MetadataAddress;
                // options.Audience = identityOptions.ClientId;

                options.Authority = configuration["Infrastructure:Keycloak:Authority"];
                options.Audience = configuration["Infrastructure:Keycloak:Audience"];
                options.RequireHttpsMetadata = false;
                options.IncludeErrorDetails = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = false,
                    ValidAudience = configuration["Infrastructure:Keycloak:Audience"],
                    
                    SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                    {
                        var jwt = new Microsoft.IdentityModel.JsonWebTokens.JsonWebToken(token);

                        return jwt;
                    },
                };
                
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();

                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";

                        // Debug only for security reasons
                        // return c.Response.WriteAsync(c.Exception.ToString());

                        return c.Response.WriteAsync($"An error occured processing your authentication - {c.Exception.Message}");
                    }
                };
            })
            .AddOAuth(allegroAzTeamTenantOAuthAuthenticationScheme, options =>
            {
                options.ClientId = allegroOptions.AzTeamTenant.ClientId;
                options.ClientSecret = allegroOptions.AzTeamTenant.ClientSecret;
                options.CallbackPath = new PathString(allegroOptions.AzTeamTenant.RedirectUri);
                options.SignInScheme = allegroAzTeamTenantCookieAuthenticationScheme;
                
                ConfigureCommonOAuthOptions(services, options, allegroOptions, identityOptions, clientUrlAppRedirect);
            })
            .AddOAuth(allegroMyTestTenantOAuthAuthenticationScheme, options =>
            {
                options.ClientId = allegroOptions.MyTestTenant.ClientId;
                options.ClientSecret = allegroOptions.MyTestTenant.ClientSecret;
                options.CallbackPath = new PathString(allegroOptions.MyTestTenant.RedirectUri);
                options.SignInScheme = allegroMyTestTenantCookieAuthenticationScheme;
                
                ConfigureCommonOAuthOptions(services, options, allegroOptions, identityOptions, clientUrlAppRedirect);
            })
            .AddOAuth(allegroMebleplTenantOAuthAuthenticationScheme, options =>
            {
                options.ClientId = allegroOptions.MebleplTenant.ClientId;
                options.ClientSecret = allegroOptions.MebleplTenant.ClientSecret;
                options.CallbackPath = new PathString(allegroOptions.MebleplTenant.RedirectUri);
                options.SignInScheme = allegroMebleplTenantCookieAuthenticationScheme;
                
                ConfigureCommonOAuthOptions(services, options, allegroOptions, identityOptions, clientUrlAppRedirect);
            });
        
        return services;
    }

    private static void ConfigureCommonOAuthOptions(
        IServiceCollection services,
        OAuthOptions options,
        AllegroOptions allegroOptions,
        IdentityOptions identityOptions,
        string clientUrlAppRedirect)
    {
        options.AuthorizationEndpoint = allegroOptions.AuthorizationEndpoint;
        options.TokenEndpoint = allegroOptions.RedirectUri;
        
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
                // TODO: This can be removed after introducment of the new authentication flow using Keycloak
                
                // Account exists, so we don't need to force login
                var jwtToken = GenerateJwtToken(tenantId, identityOptions);
                
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
            
            var accessToken = ctx.AccessToken;
            var refreshToken = ctx.RefreshToken;
            
            // TODO: We could save new tenant account in DB
            // ...
            
            
            // TODO: This can be removed after introducment of the new authentication flow using Keycloak
            
            var jwtToken = GenerateJwtToken(tenantId, identityOptions);

            ctx.Properties.StoreTokens([
                new AuthenticationToken
                {
                    Name = "integrator_access_token",
                    Value = jwtToken
                }
            ]);
            
            
            return Task.CompletedTask;
        };
    }

    private static string GenerateJwtToken(string tenantId, IdentityOptions identityOptions)
    {
        // var claims = new List<Claim>
        // {
        //     new(UserClaimType.ShipXOrganizationId, shipXOptions.OrganizationId.ToString()),
        //     new(UserClaimType.TenantId, tenantId),
        //     new(UserClaimType.AuthorizationProviderType, ShopProviderType.Allegro.ToString())
        // };
                
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: identityOptions.Issuer,
            audience: identityOptions.Audience,
            // claims: claims,
            expires: DateTime.UtcNow.AddHours(identityOptions.ExpiresInHours),
            signingCredentials:  new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identityOptions.PrivateKey)),
                SecurityAlgorithms.HmacSha256)
        );

        return jwtTokenHandler.WriteToken(jwtSecurityToken);
    }
}