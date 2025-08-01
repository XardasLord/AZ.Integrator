using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.ChangeOrderStatus;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment;
using Hangfire;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.EventHandlers.InpostTrackingNumberAssigned;

public class ChangeOrderStatusInExternalSystem(IBackgroundJobClient backgroundJobClient)
    : INotificationHandler<InpostTrackingNumbersAssigned>
{
    public ValueTask Handle(InpostTrackingNumbersAssigned notification, CancellationToken cancellationToken)
    {
        var shopProvider = TenantHelper.GetShopProviderType(notification.TenantId);

        if (shopProvider == ShopProviderType.Allegro)
        {
            backgroundJobClient.Enqueue<ChangeAllegroOrderStatusJob>(
                job => job.Execute(new ChangeAllegroOrderStatusJobCommand
                {
                    OrderNumber = Guid.Parse(notification.ExternalOrderNumber),
                    OrderStatus = AllegroFulfillmentStatusEnum.ReadyForShipment.Value,
                    TenantId = notification.TenantId
                }, null));
        }
        else if (shopProvider == ShopProviderType.Erli)
        {
            // Erli does not support changing Order's status
        }
        else if (shopProvider == ShopProviderType.Shopify)
        {
            // TODO: Potentially implement Shopify order status change
        }

        return new ValueTask();
    }
}