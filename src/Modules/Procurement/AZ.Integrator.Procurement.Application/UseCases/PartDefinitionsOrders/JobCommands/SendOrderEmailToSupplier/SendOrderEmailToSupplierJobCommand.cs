using AZ.Integrator.Procurement.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.JobCommands.SendOrderEmailToSupplier;

public class SendOrderEmailToSupplierJobCommand : JobCommandBase
{
    public uint SupplierId { get; set; }
    public string OrderNumber { get; set; }
    public Guid TenantId { get; set; }
}