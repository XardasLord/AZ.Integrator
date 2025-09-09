using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Shipments.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.GenerateInvoice;

public class GenerateInvoiceJobCommand : JobCommandBase
{
    public string ShippingNumber { get; set; }
    public string ExternalOrderNumber { get; set; }
    public string TenantId { get; set; }
    public ShopProviderType ShopProvider { get; set; }
}