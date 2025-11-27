namespace AZ.Integrator.Integrations.Contracts.RequestDtos;

public record AddErliIntegrationRequest(
    string ApiKey,
    string DisplayName);

public record UpdateErliIntegrationRequest(
    string SourceSystemId,
    string ApiKey,
    string DisplayName);
