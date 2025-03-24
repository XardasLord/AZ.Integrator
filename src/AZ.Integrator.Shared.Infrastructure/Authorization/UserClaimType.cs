using System.Security.Claims;

namespace AZ.Integrator.Shared.Infrastructure.Authorization;

public static class UserClaimType
{
    public const string UserId = ClaimTypes.NameIdentifier;
    public const string Name = "name";
    public const string UserName = "preferred_username";
    public const string Email = ClaimTypes.Email;
    public const string Roles = ClaimTypes.Role;
}