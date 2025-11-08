using AZ.Integrator.Procurement.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.JobCommands.MarkOrderAsSent;

public class MarkOrderAsSentJob(IMediator mediator) : JobBase<MarkOrderAsSentJobCommand>(mediator);