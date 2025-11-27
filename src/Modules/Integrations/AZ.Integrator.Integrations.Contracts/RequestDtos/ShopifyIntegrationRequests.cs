namespace AZ.Integrator.Integrations.Contracts.RequestDtos;

public record AddShopifyIntegrationRequest(
    string ApiUrl,
    string AdminApiToken,
    string DisplayName);

public record UpdateShopifyIntegrationRequest(
    string SourceSystemId,
    string ApiUrl,
    string AdminApiToken,
    string DisplayName);