namespace AZ.Integrator.Integrations.Contracts.RequestDtos;

public record AddInpostIntegrationRequest(
    int OrganizationId,
    string AccessToken,
    string DisplayName,
    string SenderName,
    string SenderCompanyName,
    string SenderFirstName,
    string SenderLastName,
    string SenderEmail,
    string SenderPhone,
    string SenderAddressStreet,
    string SenderAddressBuildingNumber,
    string SenderAddressCity,
    string SenderAddressPostCode,
    string SenderAddressCountryCode);

public record UpdateInpostIntegrationRequest(
    int OrganizationId,
    string AccessToken,
    string DisplayName,
    string SenderName,
    string SenderCompanyName,
    string SenderFirstName,
    string SenderLastName,
    string SenderEmail,
    string SenderPhone,
    string SenderAddressStreet,
    string SenderAddressBuildingNumber,
    string SenderAddressCity,
    string SenderAddressPostCode,
    string SenderAddressCountryCode);
