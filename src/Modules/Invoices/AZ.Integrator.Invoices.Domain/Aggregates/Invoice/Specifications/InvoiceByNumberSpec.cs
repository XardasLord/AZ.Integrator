using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice.ValueObjects;

namespace AZ.Integrator.Invoices.Domain.Aggregates.Invoice.Specifications;

public sealed class InvoiceByNumberSpec : Specification<Invoice>, ISingleResultSpecification<Invoice>
{
    public InvoiceByNumberSpec(string invoiceId, string externalOrderNumber, int invoiceProvider, Guid tenantId, string sourceSystemId)
    {
        Query
            .Where(x => x.ExternalId == int.Parse(invoiceId))
            .Where(x => x.ExternalOrderNumber == externalOrderNumber)
            .Where(x => x.InvoiceProvider == (InvoiceProvider)invoiceProvider)
            .Where(x => x.CreationInformation.SourceSystemId == sourceSystemId)
            .Where(x => x.CreationInformation.TenantId == new TenantId(tenantId));
    }
}