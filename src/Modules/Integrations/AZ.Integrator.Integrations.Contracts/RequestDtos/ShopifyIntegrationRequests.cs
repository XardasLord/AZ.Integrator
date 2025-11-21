namespace AZ.Integrator.Integrations.Contracts.RequestDtos;

public record AddShopifyIntegrationRequest(
    string SourceSystemId,
    string ApiUrl,
    string AdminApiToken,
    string DisplayName);