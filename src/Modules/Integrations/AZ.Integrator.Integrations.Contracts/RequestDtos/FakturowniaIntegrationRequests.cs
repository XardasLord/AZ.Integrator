namespace AZ.Integrator.Integrations.Contracts.RequestDtos;

public record AddFakturowniaIntegrationRequest(
    string ApiKey,
    string ApiUrl,
    string DisplayName);

public record UpdateFakturowniaIntegrationRequest(
    string SourceSystemId,
    string ApiKey,
    string ApiUrl,
    string DisplayName);
