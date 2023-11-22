using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AZ.Integrator.Shared.Infrastructure.Identity;

public class TokenService
{
    private readonly IConfiguration _configuration;
    private const int ExpirationMinutes = 30;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string CreateToken(IdentityUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        var token = CreateJwtToken(
            CreateClaims(user),
            CreateSigningCredentials(),
            expiration);
            
        var tokenHandler = new JwtSecurityTokenHandler();
            
        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
        DateTime expiration) =>
        new(
            _configuration["Infrastructure:Identity:Issuer"],
            _configuration["Infrastructure:Identity:Audience"],
            claims,
            expires: expiration,
            signingCredentials: credentials);

    private List<Claim> CreateClaims(IdentityUser user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.Email, user.Email)
            };
            return claims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Infrastructure:Identity:PrivateKey"]!)),
            SecurityAlgorithms.HmacSha256);
    }
}