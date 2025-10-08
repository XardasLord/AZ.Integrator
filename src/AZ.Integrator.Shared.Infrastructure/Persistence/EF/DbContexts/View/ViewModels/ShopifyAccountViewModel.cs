namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;

public class ShopifyAccountViewModel
{
    public string TenantId { get; set; }
    public string SourceSystemId { get; set; }
    public string ApiUrl { get; set; }
    public string AdminApiToken { get; set; }
}