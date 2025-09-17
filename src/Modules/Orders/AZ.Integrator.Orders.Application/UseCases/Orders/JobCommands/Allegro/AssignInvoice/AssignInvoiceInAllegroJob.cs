using AZ.Integrator.Orders.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.AssignInvoice;

public class AssignInvoiceInAllegroJob(IMediator mediator) : JobBase<AssignInvoiceInAllegroJobCommand>(mediator);