namespace AZ.Integrator.Shared.Infrastructure.Authorization;

public static class UserClaimType
{
    public const string UserId = "sub";
    public const string UserName = "name";
    public const string Role = "role";
    public const string TenantId = "tenant_id";
    public const string AllegroAccessToken = "allegro_access_token";
    public const string AllegroRefreshToken = "allegro_refresh_token";
    public const string ShipXOrganizationId = "shipx_organization_id";
}