using AZ.Integrator.Orders.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Erli.AssignTrackingNumbers;

public class AssignTrackingNumbersInErliJob(IMediator mediator) : JobBase<AssignTrackingNumbersInErliJobCommand>(mediator);