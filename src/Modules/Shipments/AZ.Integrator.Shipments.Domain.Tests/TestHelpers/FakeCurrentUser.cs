using AZ.Integrator.Domain.Abstractions;

namespace AZ.Integrator.Shipments.Domain.Tests.TestHelpers;

internal sealed class FakeCurrentUser : ICurrentUser
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "Test User";
    public string UserName { get; set; } = "test.user";
    public string TenantName { get; set; } = "Test Tenant";
    public Guid TenantId { get; set; } = Guid.NewGuid();
    public IReadOnlyCollection<string> Roles { get; set; } = new List<string>();
    public IReadOnlyCollection<string> AppScopes { get; set; } = new List<string>();
}

