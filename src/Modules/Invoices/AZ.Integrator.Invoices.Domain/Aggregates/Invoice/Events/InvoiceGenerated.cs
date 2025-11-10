using Mediator;

namespace AZ.Integrator.Invoices.Domain.Aggregates.Invoice.Events;

public record InvoiceGenerated(
    string InvoiceExternalId,
    string InvoiceNumber,
    string ExternalOrderNumber,
    int InvoiceProvider,
    Guid TenantId,
    string SourceSystemId) : INotification;