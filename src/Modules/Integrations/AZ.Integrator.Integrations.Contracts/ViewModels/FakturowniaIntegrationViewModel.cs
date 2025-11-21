namespace AZ.Integrator.Integrations.Contracts.ViewModels;

public class FakturowniaIntegrationViewModel
{
    public Guid TenantId { get; set; }
    public string SourceSystemId { get; set; }
    public string ApiKey { get; set; }
    public string ApiUrl { get; set; }
    public string DisplayName { get; set; }
    public bool IsEnabled { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}