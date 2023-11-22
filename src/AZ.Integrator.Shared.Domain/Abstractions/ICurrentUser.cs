namespace AZ.Integrator.Domain.Abstractions;

public interface ICurrentUser
{
    Guid UserId { get; }
    string UserName { get; }
    string Role { get; }
    IReadOnlyCollection<string> AppScopes { get; }
    string AllegroAccessToken { get; }
    string AllegroRefreshToken { get; }
    int ShipXOrganizationId { get; }
}