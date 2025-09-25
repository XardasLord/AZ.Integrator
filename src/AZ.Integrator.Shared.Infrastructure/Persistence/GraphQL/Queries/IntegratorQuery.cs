using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Shipment;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class IntegratorQuery
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