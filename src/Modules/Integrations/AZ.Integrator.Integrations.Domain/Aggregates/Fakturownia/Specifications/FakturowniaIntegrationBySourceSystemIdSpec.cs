using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Integrations.Domain.Aggregates.Fakturownia.Specifications;

public sealed class FakturowniaIntegrationBySourceSystemIdSpec : Specification<FakturowniaIntegration>, ISingleResultSpecification<FakturowniaIntegration>
{
    public FakturowniaIntegrationBySourceSystemIdSpec(TenantId tenantId, SourceSystemId sourceSystemId)
    {
        Query
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.SourceSystemId == sourceSystemId);
    }
}