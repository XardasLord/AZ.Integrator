using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Invoices.Domain.Events;
using AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.AssignInvoice;
using AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Shopify.AssignInvoice;
using Hangfire;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.EventHandlers.InvoiceCreated;

public class AssignInvoiceToOrderInExternalSystem(IBackgroundJobClient backgroundJobClient) 
    : INotificationHandler<InvoiceGenerated>
{
    public ValueTask Handle(InvoiceGenerated notification, CancellationToken cancellationToken)
    {
        var shopProvider = notification.TenantId.GetShopProviderType();
        
        switch (shopProvider)
        {
            case ShopProviderType.Allegro:
                backgroundJobClient.Enqueue<AssignInvoiceInAllegroJob>(
                    job => job.Execute(new AssignInvoiceInAllegroJobCommand
                    {
                        OrderNumber = Guid.Parse(notification.ExternalOrderNumber),
                        InvoiceNumber = notification.InvoiceNumber,
                        ExternalInvoiceId = notification.InvoiceExternalId,
                        TenantId = notification.TenantId
                    }, null));
                break;
            case ShopProviderType.Erli:
                // Currently not available in Erli API
                break;
            case ShopProviderType.Shopify:
                backgroundJobClient.Enqueue<AssignInvoiceInShopifyJob>(
                    job => job.Execute(new AssignInvoiceInShopifyJobCommand
                    {
                        OrderNumber = notification.ExternalOrderNumber,
                        InvoiceNumber = notification.InvoiceNumber,
                        ExternalInvoiceId = notification.InvoiceExternalId,
                        TenantId = notification.TenantId
                    }, null));
                break;
        }
        
        return new ValueTask();
    }
}