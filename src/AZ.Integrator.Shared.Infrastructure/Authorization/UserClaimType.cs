namespace AZ.Integrator.Shared.Infrastructure.Authorization;

public static class UserClaimType
{
    public const string UserId = "sub";
    public const string Name = "name";
    public const string UserName = "preferred_username";
    public const string Email = "email";
    public const string Roles = "roles";
    // public const string TenantId = "tenant_id";
    // public const string ShipXOrganizationId = "shipx_organization_id";
    // public const string AuthorizationProviderType = "auth_provider_type";
}