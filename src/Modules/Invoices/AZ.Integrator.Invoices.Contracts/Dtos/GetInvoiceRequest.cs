namespace AZ.Integrator.Invoices.Contracts.Dtos;

public sealed record GetInvoiceRequest(
    string InvoiceId,
    string ExternalOrderId,
    int InvoiceProvider,
    Guid TenantId,
    string SourceSystemId);