using AZ.Integrator.Orders.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.AssignTrackingNumbers;

public class AssignTrackingNumbersInAllegroJob(IMediator mediator) : JobBase<AssignTrackingNumbersInAllegroJobCommand>(mediator);