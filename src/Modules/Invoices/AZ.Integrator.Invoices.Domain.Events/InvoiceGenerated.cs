using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using Mediator;

namespace AZ.Integrator.Invoices.Domain.Events;

public record InvoiceGenerated(
    string InvoiceExternalId,
    string InvoiceNumber,
    ExternalOrderNumber ExternalOrderNumber,
    int InvoiceProvider,
    TenantId TenantId) : INotification;