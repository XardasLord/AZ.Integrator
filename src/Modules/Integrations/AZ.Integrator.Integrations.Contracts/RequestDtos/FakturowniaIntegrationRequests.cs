namespace AZ.Integrator.Integrations.Contracts.RequestDtos;

public record AddFakturowniaIntegrationRequest(
    string SourceSystemId,
    string ApiKey,
    string ApiUrl,
    string DisplayName);