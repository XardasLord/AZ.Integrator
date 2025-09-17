namespace AZ.Integrator.Invoices.Contracts.Dtos;

public sealed record GetInvoiceRequest(
    string InvoiceId,
    string ExternalOrderId,
    int InvoiceProvider,
    string TenantId);