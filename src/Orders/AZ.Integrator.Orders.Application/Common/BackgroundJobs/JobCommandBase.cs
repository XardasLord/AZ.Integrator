using Hangfire.Server;

namespace AZ.Integrator.Orders.Application.Common.BackgroundJobs;

public abstract class JobCommandBase : MediatR.IRequest
{
    public PerformContext PerformContext { get; set; }
}