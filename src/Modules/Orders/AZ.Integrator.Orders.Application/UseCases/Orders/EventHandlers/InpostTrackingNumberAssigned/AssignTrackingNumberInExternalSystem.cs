using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.AssignTrackingNumbers;
using AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Erli.AssignTrackingNumbers;
using AZ.Integrator.Shared.Application.ExternalServices.Erli;
using AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment;
using Hangfire;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.EventHandlers.InpostTrackingNumberAssigned;

public class AssignTrackingNumberInExternalSystem(IBackgroundJobClient backgroundJobClient)
    : INotificationHandler<InpostTrackingNumbersAssigned>
{
    public ValueTask Handle(InpostTrackingNumbersAssigned notification, CancellationToken cancellationToken)
    {
        var shopProvider = TenantHelper.GetShopProviderType(notification.TenantId);

        if (shopProvider == ShopProviderType.Allegro)
        {
            backgroundJobClient.Enqueue<AssignTrackingNumbersInAllegroJob>(
                job => job.Execute(new AssignTrackingNumbersInAllegroJobCommand
                {
                    OrderNumber = Guid.Parse(notification.ExternalOrderNumber),
                    TrackingNumbers = notification.TrackingNumbers,
                    TenantId = notification.TenantId
                }, null));
        }
        else if (shopProvider == ShopProviderType.Erli)
        {
            backgroundJobClient.Enqueue<AssignTrackingNumbersInErliJob>(
                job => job.Execute(new AssignTrackingNumbersInErliJobCommand
                {
                    OrderNumber = notification.ExternalOrderNumber,
                    TrackingNumbers = notification.TrackingNumbers,
                    DeliveryTrackingStatus = ErliDeliveryTrackingStatusEnum.ReadyToSend.Name,
                    Vendor = ErliDeliveryTrackingVendorEnum.InPost.Name,
                    TenantId = notification.TenantId
                }, null));
        }
        else if (shopProvider == ShopProviderType.Shopify)
        {
            // TODO: Potentially implement tracking number assignment for Shopify
        }

        return new ValueTask();
    }
}