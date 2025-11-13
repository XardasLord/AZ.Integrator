using System.Security.Claims;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Authorization;
using AZ.Integrator.Shared.Infrastructure.Authorization.Scopes;
using Microsoft.AspNetCore.Http;

namespace AZ.Integrator.Shared.Infrastructure.UserInfo;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private static string System => "System";
    private static Guid SystemGuid => Guid.Parse("00000000-0000-0000-0000-1234567890ab");

    public Guid UserId => GetClaimValue(UserClaimType.UserId) is null ? SystemGuid : Guid.Parse(GetClaimValue(UserClaimType.UserId));
    public string Name => GetClaimValue(UserClaimType.Name) ?? System;
    public string UserName => GetClaimValue(UserClaimType.UserName) ?? System;
    public string TenantName => GetClaimValue(UserClaimType.TenantName) ?? System;
    public Guid TenantId => GetClaimValue(UserClaimType.TenantId) is null ? Guid.Empty : Guid.Parse(GetClaimValue(UserClaimType.TenantId));
    public string Email => GetClaimValue(UserClaimType.Email) ?? System;
    
    public IReadOnlyCollection<string> Roles => httpContextAccessor.HttpContext?.User.Claims
        .Where(c => c.Type == UserClaimType.Roles)
        .Select(c => c.Value)
        .Distinct()
        .ToList();

    public IReadOnlyCollection<string> AppScopes => httpContextAccessor.HttpContext?.User.Claims
        .Where(c => c.Type == AppClaims.Scopes)
        .Select(c => c.Value)
        .Distinct()
        .ToList();

    public bool IsMasterAdmin() => Roles.Any(x => x == "MASTER_ADMIN");

    private string GetClaimValue(string claimType) => httpContextAccessor.HttpContext?.User.FindFirstValue(claimType);
}