using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.AssignTrackingNumbers;
using AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Erli.AssignTrackingNumbers;
using AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Shopify.AssignTrackingNumbers;
using AZ.Integrator.Shared.Application.ExternalServices.Erli;
using AZ.Integrator.Shared.Application.ExternalServices.Shopify;
using AZ.Integrator.Shipments.Contracts.IntegrationEvents;
using Hangfire;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.EventHandlers.InpostTrackingNumberAssigned;

public class AssignTrackingNumberInExternalSystem(IBackgroundJobClient backgroundJobClient)
    : INotificationHandler<InpostTrackingNumbersAssignedV1>
{
    public ValueTask Handle(InpostTrackingNumbersAssignedV1 notification, CancellationToken cancellationToken)
    {
        var shopProvider = ShopProviderHelper.GetShopProviderType(notification.SourceSystemId);

        if (shopProvider == ShopProviderType.Allegro)
        {
            backgroundJobClient.Enqueue<AssignTrackingNumbersInAllegroJob>(
                job => job.Execute(new AssignTrackingNumbersInAllegroJobCommand
                {
                    OrderNumber = Guid.Parse(notification.ExternalOrderNumber),
                    TrackingNumbers = notification.TrackingNumbers,
                    TenantId = notification.TenantId,
                    SourceSystemId = notification.SourceSystemId
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
                    TenantId = notification.TenantId,
                    SourceSystemId = notification.SourceSystemId
                }, null));
        }
        else if (shopProvider == ShopProviderType.Shopify)
        {
            backgroundJobClient.Enqueue<AssignTrackingNumbersInShopifyJob>(
                job => job.Execute(new AssignTrackingNumbersInShopifyJobCommand
                {
                    OrderNumber = notification.ExternalOrderNumber,
                    TrackingNumbers = notification.TrackingNumbers,
                    Vendor = ShopifyDeliveryTrackingVendorEnum.InPost.Name,
                    TenantId = notification.TenantId,
                    SourceSystemId = notification.SourceSystemId
                }, null));
        }

        return new ValueTask();
    }
}