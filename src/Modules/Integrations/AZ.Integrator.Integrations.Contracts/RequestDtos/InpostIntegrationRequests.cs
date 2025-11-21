namespace AZ.Integrator.Integrations.Contracts.RequestDtos;

public record AddInpostIntegrationRequest(
    int OrganizationId,
    string AccessToken,
    string DisplayName);