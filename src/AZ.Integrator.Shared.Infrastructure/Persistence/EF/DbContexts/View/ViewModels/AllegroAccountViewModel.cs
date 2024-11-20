namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;

public class AllegroAccountViewModel
{
    public string TenantId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RedirectUri { get; set; }
}