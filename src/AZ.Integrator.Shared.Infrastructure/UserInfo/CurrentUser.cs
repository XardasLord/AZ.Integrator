using System.Security.Claims;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Authorization;
using AZ.Integrator.Shared.Infrastructure.Authorization.Scopes;
using Microsoft.AspNetCore.Http;

namespace AZ.Integrator.Shared.Infrastructure.UserInfo;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    private static string System => "System";

    public Guid UserId => GetClaimValue(UserClaimType.UserId) is null ? Guid.Empty : Guid.Parse(GetClaimValue(UserClaimType.UserId));
    public string UserName => GetClaimValue(UserClaimType.UserName) ?? System;
    public string Role => GetClaimValue(UserClaimType.Role) ?? System;

    public string AllegroAccessToken => GetClaimValue(UserClaimType.AllegroAccessToken) ?? "";
    public string AllegroRefreshToken => GetClaimValue(UserClaimType.AllegroRefreshToken) ?? "";
    public int ShipXOrganizationId => GetClaimValueAsNumber(UserClaimType.ShipXOrganizationId) ?? -1;

    public IReadOnlyCollection<string> AppScopes => _httpContextAccessor.HttpContext?.User.Claims
        .Where(c => c.Type == AppClaims.Scopes)
        .Select(c => c.Value)
        .Distinct()
        .ToList();

    private string GetClaimValue(string claimType) => _httpContextAccessor.HttpContext?.User.FindFirstValue(claimType);
    private int? GetClaimValueAsNumber(string claimType) => int.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(claimType) ?? "");
}