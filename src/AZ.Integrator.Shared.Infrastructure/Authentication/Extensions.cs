using System.IdentityModel.Tokens.Jwt;
using AZ.Integrator.Integrations.Contracts;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
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
            
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

        const string allegroAboxynCookieAuthenticationScheme = $"{CookieAuthenticationDefaults.AuthenticationScheme}-aboxyn";
        
        const string allegroAboxynOAuthAuthenticationScheme = "allegro-aboxyn";
        
        services
            .AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(allegroAboxynCookieAuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
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
                        var jwt = new JsonWebToken(token);

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

                        return c.Response.WriteAsync($"An error occured processing your authentication - {c.Exception.Message}");
                    }
                };
            })
            .AddOAuth(allegroAboxynOAuthAuthenticationScheme, options =>
            {
                options.ClientId = allegroOptions.ClientId;
                options.ClientSecret = allegroOptions.ClientSecret;
                options.CallbackPath = new PathString(allegroOptions.RedirectUri);
                options.SignInScheme = allegroAboxynCookieAuthenticationScheme;
                
                options.AuthorizationEndpoint = allegroOptions.AuthorizationEndpoint;
                options.TokenEndpoint = allegroOptions.TokenEndpoint;
                
                options.SaveTokens = true;

                var innerHandler = new HttpClientHandler();
                options.BackchannelHttpHandler = new TokenExchangeAuthorizingHandler(innerHandler, options);
                
                options.Events.OnRedirectToAuthorizationEndpoint = context =>
                {
                    // Do URL-a, który middleware już zbudował,
                    // dokładamy &prompt=confirm (albo inny zgodnie z docs Allegro)
                    var uri = context.RedirectUri;

                    if (!uri.Contains("prompt="))
                    {
                        uri += (uri.Contains('?') ? "&" : "?") + "prompt=confirm";
                    }

                    context.Response.Redirect(uri);
                    return Task.CompletedTask;
                };


                options.Events.OnCreatingTicket = async ctx =>
                {
                    var tenantId = Guid.Parse(ctx.Properties.Items["tenant_id"]!);
                    
                    var accessToken = ctx.AccessToken;
                    var refreshToken = ctx.RefreshToken;
                    var expiresAt = DateTime.Now.Add(ctx.ExpiresIn ?? TimeSpan.FromHours(1));
                    
                    var serviceProvider = services.BuildServiceProvider();
                    var integrationsWriteFacade = serviceProvider.GetRequiredService<IIntegrationsWriteFacade>();

                    await integrationsWriteFacade.AddAllegroIntegrationAsync(
                        tenantId,
                        new AllegroIntegrationCreateModel 
                        {
                            AccessToken = accessToken!,
                            RefreshToken = refreshToken!,
                            ExpiresAt = expiresAt
                        });
                };
            });
        
        return services;
    }
}