using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using AZ.Integrator.Shipments.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Shipments.Infrastructure.Persistence.EF.View.ViewModels;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AZ.Integrator.Shipments.Infrastructure.Persistence.GraphQL.QueryResolvers;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class ShipmentsViewResolver
{
    [UseProjection]
    [UseFiltering]
    public IQueryable<InpostShipmentViewModel> GetInpostShipments([Service] ShipmentDataViewContext dataViewContext) 
        => dataViewContext.InpostShipments.AsQueryable();

    [UseProjection]
    [UseFiltering]
    public IQueryable<DpdShipmentViewModel> GetDpdShipments([Service] ShipmentDataViewContext dataViewContext) 
        => dataViewContext.DpdShipments.AsQueryable();

    [UseProjection]
    [UseFiltering]
    public IQueryable<ShipmentViewModel> GetShipments([Service] ShipmentDataViewContext dataViewContext) 
        => dataViewContext.Shipments.AsQueryable();
}