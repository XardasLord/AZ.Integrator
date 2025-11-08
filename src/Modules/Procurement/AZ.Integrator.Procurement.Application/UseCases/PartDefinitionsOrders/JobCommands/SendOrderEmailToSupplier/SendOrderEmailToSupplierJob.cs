using AZ.Integrator.Procurement.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.JobCommands.SendOrderEmailToSupplier;

public class SendOrderEmailToSupplierJob(IMediator mediator) : JobBase<SendOrderEmailToSupplierJobCommand>(mediator);