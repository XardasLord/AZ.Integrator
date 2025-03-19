namespace AZ.Integrator.Domain.Abstractions;

public interface ICurrentUser
{
    Guid UserId { get; }
    string Name { get; }
    string UserName { get; }
    IReadOnlyCollection<string> Roles { get; }
    IReadOnlyCollection<string> AppScopes { get; }
}