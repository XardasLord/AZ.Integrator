using AZ.Integrator.Procurement.Contracts.PartDefinitionOrders;
using AZ.Integrator.Procurement.Contracts.Suppliers;
using AZ.Integrator.Procurement.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AZ.Integrator.Procurement.Infrastructure.Persistence.GraphQL.QueryResolvers;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class ProcurementViewResolver
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<SupplierViewModel> GetSuppliers([Service] ProcurementDataViewContext dataViewContext) 
        => dataViewContext.Suppliers.AsQueryable();
    
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<PartDefinitionsOrderViewModel> GetPartDefinitionOrders([Service] ProcurementDataViewContext dataViewContext) 
        => dataViewContext.Orders.AsQueryable();
}