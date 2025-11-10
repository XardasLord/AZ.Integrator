using AZ.Integrator.Invoices.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Invoices.Infrastructure.Persistence.EF.View.ViewModels;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AZ.Integrator.Invoices.Infrastructure.Persistence.GraphQL.QueryResolvers;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class InvoicesViewResolver
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<InvoiceViewModel> GetInvoices(InvoiceDataViewContext dataViewContext) 
        => dataViewContext.Invoices.AsQueryable();
}