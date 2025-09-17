using AZ.Integrator.Shipments.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.GenerateInvoice;

public class GenerateInvoiceJob(IMediator mediator) : JobBase<GenerateInvoiceJobCommand>(mediator);