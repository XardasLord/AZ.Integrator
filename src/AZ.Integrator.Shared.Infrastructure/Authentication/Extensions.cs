﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AZ.Integrator.Shared.Infrastructure.Authorization;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        
        services
            .AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = "allegro";
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                // add an instance of the patched manager to the options:
                options.CookieManager = new ChunkingCookieManager();

                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
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
            .AddOAuth("allegro", options =>
            {
                options.ClientId = allegroOptions.ClientId;
                options.ClientSecret = allegroOptions.ClientSecret;
                options.CallbackPath = new PathString("/auth/allegro-auth-callback");
                options.AuthorizationEndpoint = allegroOptions.AuthorizationEndpoint;
                options.TokenEndpoint = allegroOptions.TokenEndpoint;

                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.SaveTokens = true;
                
                options.CorrelationCookie.HttpOnly = true;
                options.CorrelationCookie.SameSite = SameSiteMode.Lax;
                
                var innerHandler = new HttpClientHandler();
                options.BackchannelHttpHandler = new TokenExchangeAuthorizingHandler(innerHandler, options);
                
                options.Events.OnCreatingTicket = ctx =>
                {
                    var allegroAccessToken = ctx.AccessToken;
                    // var allegroRefreshToken = ctx.RefreshToken;
                    
                    var claims = new List<Claim>
                    {
                        new(UserClaimType.AllegroAccessToken, allegroAccessToken),
                        // new(UserClaimType.AllegroRefreshToken, allegroRefreshToken),
                        new(UserClaimType.ShipXOrganizationId, configuration["Infrastructure:ShipX:OrganizationId"])
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
            });
        
        return services;
    }
}