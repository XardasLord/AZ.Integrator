using System.Security.Claims;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Authorization;
using AZ.Integrator.Shared.Infrastructure.Authorization.Scopes;
using Microsoft.AspNetCore.Http;

namespace AZ.Integrator.Shared.Infrastructure.UserInfo;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private static string System => "System";

    public Guid UserId => GetClaimValue(UserClaimType.UserId) is null ? Guid.Empty : Guid.Parse(GetClaimValue(UserClaimType.UserId));
    public string Name => GetClaimValue(UserClaimType.Name) ?? System;
    public string UserName => GetClaimValue(UserClaimType.UserName) ?? System;
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

    private string GetClaimValue(string claimType) => httpContextAccessor.HttpContext?.User.FindFirstValue(claimType);
}