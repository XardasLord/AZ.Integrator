using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Integrations.Domain.Aggregates.Erli.Specifications;

public sealed class ErliIntegrationBySourceSystemIdSpec : Specification<ErliIntegration>, ISingleResultSpecification<ErliIntegration>
{
    public ErliIntegrationBySourceSystemIdSpec(TenantId tenantId, SourceSystemId sourceSystemId)
    {
        Query
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.SourceSystemId == sourceSystemId);
    }
}