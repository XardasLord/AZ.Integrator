using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Invoices.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Invoices.Infrastructure.Persistence.EF.View.ViewModels;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AZ.Integrator.Invoices.Infrastructure.Persistence.GraphQL.QueryResolvers;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class InvoicesViewResolver(ICurrentUser currentUser)
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<InvoiceViewModel> GetInvoices(InvoiceDataViewContext dataViewContext) 
        => dataViewContext.Invoices
            .Where(x => x.TenantId == currentUser.TenantId)
            .AsQueryable();
}