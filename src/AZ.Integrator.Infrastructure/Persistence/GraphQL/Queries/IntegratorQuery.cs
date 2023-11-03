using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Infrastructure.Persistence.EF.Configurations.View.ViewModels;
using AZ.Integrator.Infrastructure.Persistence.EF.DbContexts;

namespace AZ.Integrator.Infrastructure.Persistence.GraphQL.Queries;

[ExtendObjectType(Name = "IntegratorQuery")]
internal class IntegratorQuery
{
    private readonly ICurrentUser _currentUser;
    
    public IntegratorQuery(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

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

    [UseProjection]
    [UseFiltering]
    public IQueryable<InvoiceViewModel> GetInvoices([Service] InvoiceDataViewContext dataViewContext) 
        => dataViewContext.Invoices.AsQueryable();
}