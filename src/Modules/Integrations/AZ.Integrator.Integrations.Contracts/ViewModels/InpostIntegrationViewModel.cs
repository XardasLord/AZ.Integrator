namespace AZ.Integrator.Integrations.Contracts.ViewModels;

public class InpostIntegrationViewModel
{
    public Guid TenantId { get; set; }
    public int OrganizationId { get; set; }
    public string AccessToken { get; set; }
    public string DisplayName { get; set; }
    public bool IsEnabled { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}