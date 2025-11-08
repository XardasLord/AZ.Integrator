using AZ.Integrator.Procurement.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.JobCommands.MarkOrderAsSent;

public class MarkOrderAsSentJobCommand : JobCommandBase
{
    public string OrderNumber { get; set; }
    public Guid TenantId { get; set; }
}