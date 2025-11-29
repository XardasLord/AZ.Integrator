using AZ.Integrator.Integrations.Contracts.ViewModels;

namespace AZ.Integrator.Integrations.Contracts;

public interface IIntegrationsWriteFacade
{
    Task<AllegroIntegrationViewModel> AddAllegroIntegrationAsync(Guid tenantId, AllegroIntegrationCreateModel createModel, CancellationToken cancellationToken = default);
}

public class AllegroIntegrationCreateModel
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime ExpiresAt { get; set; }
}