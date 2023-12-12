namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels;

public class AllegroAccountViewModel
{
    public string TenantId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}