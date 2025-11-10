using AZ.Integrator.Invoices.Contracts.IntegrationEvents;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice.Events;
using Mediator;

namespace AZ.Integrator.Invoices.Application.UseCases.Invoices.EventHandlers;

public class InvoiceGeneratedHandler(IMediator mediator) 
    : INotificationHandler<InvoiceGenerated>
{
    public async ValueTask Handle(InvoiceGenerated notification, CancellationToken cancellationToken)
    {
        // In the future, invoice URL and other details can be added to the event
        var @event = new InvoiceGeneratedV1(
            notification.InvoiceExternalId,
            notification.InvoiceNumber,
            notification.ExternalOrderNumber,
            notification.InvoiceProvider,
            notification.TenantId,
            notification.SourceSystemId);

        await mediator.Publish(@event, cancellationToken);
    }
}