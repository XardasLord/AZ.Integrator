using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using AZ.Integrator.Shipments.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Shipments.Infrastructure.Persistence.EF.View.ViewModels;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AZ.Integrator.Shipments.Infrastructure.Persistence.GraphQL.QueryResolvers;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class ShipmentsViewResolver(ICurrentUser currentUser)
{
    [UseProjection]
    [UseFiltering]
    public IQueryable<InpostShipmentViewModel> GetInpostShipments(ShipmentDataViewContext dataViewContext) 
        => dataViewContext.InpostShipments
            .Where(x => x.TenantId == currentUser.TenantId)
            .AsQueryable();

    [UseProjection]
    [UseFiltering]
    public IQueryable<DpdShipmentViewModel> GetDpdShipments(ShipmentDataViewContext dataViewContext) 
        => dataViewContext.DpdShipments
            .Where(x => x.TenantId == currentUser.TenantId)
            .AsQueryable();

    [UseProjection]
    [UseFiltering]
    public IQueryable<ShipmentViewModel> GetShipments(ShipmentDataViewContext dataViewContext) 
        => dataViewContext.Shipments
            .Where(x => x.TenantId == currentUser.TenantId)
            .AsQueryable();
}