using AZ.Integrator.Orders.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Shopify.AssignInvoice;

public class AssignInvoiceInShopifyJobCommand : JobCommandBase
{
    public string OrderNumber { get; set; }
    public string InvoiceNumber { get; set; }
    public string ExternalInvoiceId { get; set; }
    public string TenantId { get; set; }
}