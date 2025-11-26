namespace AZ.Integrator.Integrations.Application.Common.Exceptions;

public class IntegrationNotFoundException(Guid tenantId, string identifier) 
    : IntegrationsModuleApplicationException($"Integration with identifier: '{identifier}' not found for the tenant: '{tenantId}'.")
{
    public override string Code => "integration_not_found";
}

