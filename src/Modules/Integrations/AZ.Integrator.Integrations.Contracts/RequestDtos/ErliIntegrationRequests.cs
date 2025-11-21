namespace AZ.Integrator.Integrations.Contracts.RequestDtos;

public record AddErliIntegrationRequest(
    string SourceSystemId,
    string ApiKey,
    string DisplayName);