using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Integrations.Domain.Aggregates.Allegro.Specifications;

public sealed class AllegroIntegrationBySourceSystemIdSpec : Specification<AllegroIntegration>, ISingleResultSpecification<AllegroIntegration>
{
    public AllegroIntegrationBySourceSystemIdSpec(TenantId tenantId, SourceSystemId sourceSystemId)
    {
        Query
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.SourceSystemId == sourceSystemId);
    }
}