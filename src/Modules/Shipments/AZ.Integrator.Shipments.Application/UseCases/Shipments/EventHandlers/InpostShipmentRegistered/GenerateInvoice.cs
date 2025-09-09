using AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.GenerateInvoice;
using Hangfire;
using Mediator;
using InpostShipmentRegisteredEvent = AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment.InpostShipmentRegistered;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.EventHandlers.InpostShipmentRegistered;

public class GenerateInvoice(IBackgroundJobClient backgroundJobClient) 
    : INotificationHandler<InpostShipmentRegisteredEvent>
{
    public ValueTask Handle(InpostShipmentRegisteredEvent notification, CancellationToken cancellationToken)
    {
        // backgroundJobClient.Enqueue<GenerateInvoiceJob>(
        //     job => job.Execute(new GenerateInvoiceJobCommand
        //     {
        //         ShippingNumber = notification.ShipmentNumber,
        //         ExternalOrderNumber = notification.ExternalOrderNumber,
        //         TenantId = notification.TenantId,
        //         ShopProvider = notification.TenantId.GetShopProviderType()
        //     }, null));
        
        return new ValueTask();
    }
}