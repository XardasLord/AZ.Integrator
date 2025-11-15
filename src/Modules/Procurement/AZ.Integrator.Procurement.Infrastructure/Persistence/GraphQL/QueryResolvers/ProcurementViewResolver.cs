using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Platform.FeatureFlags.Abstractions;
using AZ.Integrator.Procurement.Contracts.PartDefinitionOrders;
using AZ.Integrator.Procurement.Contracts.Suppliers;
using AZ.Integrator.Procurement.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AZ.Integrator.Procurement.Infrastructure.Persistence.GraphQL.QueryResolvers;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class ProcurementViewResolver(ICurrentUser currentUser)
{
    [RequireFeatureFlag(FeatureFlagCodes.ProcurementModule)]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<SupplierViewModel> GetSuppliers(ProcurementDataViewContext dataViewContext) 
        => dataViewContext.Suppliers
            .Where(x => x.TenantId == currentUser.TenantId)
            .AsQueryable();
    
    [RequireFeatureFlag(FeatureFlagCodes.ProcurementModule)]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<PartDefinitionsOrderViewModel> GetPartDefinitionOrders(ProcurementDataViewContext dataViewContext) 
        => dataViewContext.Orders
            .Where(x => x.TenantId == currentUser.TenantId)
            .AsQueryable();
}