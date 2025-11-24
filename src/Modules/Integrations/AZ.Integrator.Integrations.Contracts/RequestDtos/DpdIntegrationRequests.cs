namespace AZ.Integrator.Integrations.Contracts.RequestDtos;

public record AddDpdIntegrationRequest(
    int Login,
    string Password,
    int MasterFid,
    string SenderName,
    string SenderCompany,
    string SenderEmail,
    string SenderPhone,
    string SenderAddress,
    string SenderAddressCity,
    string SenderAddressPostCode,
    string SenderAddressCountryCode);