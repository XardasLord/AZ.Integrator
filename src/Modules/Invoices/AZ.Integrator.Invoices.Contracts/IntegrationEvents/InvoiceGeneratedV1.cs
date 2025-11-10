using Mediator;

namespace AZ.Integrator.Invoices.Contracts.IntegrationEvents;

public record InvoiceGeneratedV1(
    string InvoiceExternalId,
    string InvoiceNumber,
    string ExternalOrderNumber,
    int InvoiceProvider,
    Guid TenantId,
    string SourceSystemId) : INotification;