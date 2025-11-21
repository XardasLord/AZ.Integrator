namespace AZ.Integrator.Integrations.Application.Common.Exceptions;

public class IntegrationAlreadyExistsException(Guid tenantId, string sourceSystemId) 
    : IntegrationsModuleApplicationException($"An integration with the specified source system ID: '{sourceSystemId}' already exists for the tenant: '{tenantId}'.")
{
    public override string Code => "integration_already_exists";
}