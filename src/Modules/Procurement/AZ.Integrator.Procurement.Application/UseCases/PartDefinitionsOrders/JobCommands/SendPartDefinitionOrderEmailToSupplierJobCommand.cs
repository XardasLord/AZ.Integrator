using AZ.Integrator.Procurement.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.JobCommands;

public class SendPartDefinitionOrderEmailToSupplierJobCommand : JobCommandBase
{
    public uint SupplierId { get; set; }
    public string OrderNumber { get; set; }
    public Guid TenantId { get; set; }
}