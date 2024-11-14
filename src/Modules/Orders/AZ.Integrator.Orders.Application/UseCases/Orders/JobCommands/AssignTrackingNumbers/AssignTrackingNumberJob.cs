using AZ.Integrator.Orders.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.AssignTrackingNumbers;

public class AssignTrackingNumberJob(IMediator mediator) : JobBase<AssignTrackingNumbersJobCommand>(mediator);