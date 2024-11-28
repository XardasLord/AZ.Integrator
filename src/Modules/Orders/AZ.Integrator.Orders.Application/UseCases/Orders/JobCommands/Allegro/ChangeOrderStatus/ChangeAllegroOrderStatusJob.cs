using AZ.Integrator.Orders.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.ChangeOrderStatus;

public class ChangeAllegroOrderStatusJob(IMediator mediator) : JobBase<ChangeAllegroOrderStatusJobCommand>(mediator);