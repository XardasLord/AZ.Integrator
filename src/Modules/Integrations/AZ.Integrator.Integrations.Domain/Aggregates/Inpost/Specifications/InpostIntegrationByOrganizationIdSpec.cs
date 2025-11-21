using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Integrations.Domain.Aggregates.Inpost.Specifications;

public sealed class InpostIntegrationByOrganizationIdSpec : Specification<InpostIntegration>, ISingleResultSpecification<InpostIntegration>
{
    public InpostIntegrationByOrganizationIdSpec(TenantId tenantId, int organizationId)
    {
        Query
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.OrganizationId == organizationId);
    }
}