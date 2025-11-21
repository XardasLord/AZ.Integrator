using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Integrations.Domain.Aggregates.Shopify.Specifications;

public sealed class ShopifyIntegrationBySourceSystemIdSpec : Specification<ShopifyIntegration>, ISingleResultSpecification<ShopifyIntegration>
{
    public ShopifyIntegrationBySourceSystemIdSpec(TenantId tenantId, SourceSystemId sourceSystemId)
    {
        Query
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.SourceSystemId == sourceSystemId);
    }
}