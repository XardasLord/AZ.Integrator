using AZ.Integrator.Orders.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Shopify.AssignTrackingNumbers;

public class AssignTrackingNumbersInShopifyJobCommand : JobCommandBase
{
    public string OrderNumber { get; set; }
    public string[] TrackingNumbers { get; set; }
    public string Vendor { get; set; }
    public Guid TenantId { get; set; }
    public string SourceSystemId { get; set; }
}