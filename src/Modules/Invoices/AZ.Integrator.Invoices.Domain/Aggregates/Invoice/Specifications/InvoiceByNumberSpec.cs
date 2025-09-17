using Ardalis.Specification;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice.ValueObjects;

namespace AZ.Integrator.Invoices.Domain.Aggregates.Invoice.Specifications;

public sealed class InvoiceByNumberSpec : Specification<Invoice>, ISingleResultSpecification<Invoice>
{
    public InvoiceByNumberSpec(string invoiceId, string externalOrderNumber, int invoiceProvider, string tenantId)
    {
        Query
            .Where(x => x.ExternalId == int.Parse(invoiceId))
            .Where(x => x.ExternalOrderNumber == externalOrderNumber)
            .Where(x => x.InvoiceProvider == (InvoiceProvider)invoiceProvider)
            .Where(x => x.CreationInformation.TenantId == tenantId);
    }
}