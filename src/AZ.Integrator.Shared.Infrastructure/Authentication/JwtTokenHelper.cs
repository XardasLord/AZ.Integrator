using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AZ.Integrator.Shared.Infrastructure.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AZ.Integrator.Shared.Infrastructure.Authentication;

public static class JwtTokenHelper
{
    public static string GenerateJwtToken(string tenantId, IConfiguration configuration)
    {
        var claims = new List<Claim>
        {
            new(UserClaimType.ShipXOrganizationId, configuration.GetSection("Infrastructure:ShipX")["OrganizationId"]),
            new(UserClaimType.TenantId, tenantId),
            new(UserClaimType.AuthorizationProviderType, AuthorizationProviderType.Erli.ToString())
        };

        var identityOptions = configuration.GetSection("Infrastructure:Identity");
                
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: identityOptions["Issuer"],
            audience: identityOptions["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(int.Parse(identityOptions["ExpiresInHours"])),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identityOptions["PrivateKey"])),
                SecurityAlgorithms.HmacSha256)
        );

        return jwtTokenHandler.WriteToken(jwtSecurityToken);
    }
}