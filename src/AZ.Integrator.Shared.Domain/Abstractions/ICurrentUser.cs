namespace AZ.Integrator.Domain.Abstractions;

public interface ICurrentUser
{
    Guid UserId { get; }
    string UserName { get; }
    string Role { get; }
    string TenantId { get; }
    IReadOnlyCollection<string> AppScopes { get; }
    int ShipXOrganizationId { get; }
}