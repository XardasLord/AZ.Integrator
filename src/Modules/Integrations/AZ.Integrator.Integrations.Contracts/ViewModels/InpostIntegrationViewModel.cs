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
    public string SenderName { get; set; }
    public string SenderCompanyName { get; set; }
    public string SenderFirstName { get; set; }
    public string SenderLastName { get; set; }
    public string SenderEmail { get; set; }
    public string SenderPhone { get; set; }
    public string SenderAddressStreet { get; set; }
    public string SenderAddressBuildingNumber { get; set; }
    public string SenderAddressCity { get; set; }
    public string SenderAddressPostCode { get; set; }
    public string SenderAddressCountryCode { get; set; }
}