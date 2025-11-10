using AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.GenerateInvoice;
using Hangfire;
using Mediator;
using InpostShipmentRegisteredEvent = AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Events.InpostShipmentRegistered;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.EventHandlers.InpostShipmentRegistered;

public class GenerateInvoice(IBackgroundJobClient backgroundJobClient) 
    : INotificationHandler<InpostShipmentRegisteredEvent>
{
    public ValueTask Handle(InpostShipmentRegisteredEvent notification, CancellationToken cancellationToken)
    {
        backgroundJobClient.Enqueue<GenerateInvoiceJob>(
            job => job.Execute(new GenerateInvoiceJobCommand
            {
                ShippingNumber = notification.ShipmentNumber,
                ExternalOrderNumber = notification.ExternalOrderNumber,
                SourceSystemId = notification.SourceSystemId,
                TenantId = notification.TenantId,
                ShopProvider = notification.ShopProviderType,
                CorrelationId = notification.CorrelationId
            }, null));
        
        return new ValueTask();
    }
}