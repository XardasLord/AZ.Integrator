using AZ.Integrator.Procurement.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.JobCommands;

public class SendPartDefinitionOrderEmailToSupplierJob(IMediator mediator) : JobBase<SendPartDefinitionOrderEmailToSupplierJobCommand>(mediator);