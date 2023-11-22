using Hangfire.Server;
using MediatR;

namespace AZ.Integrator.Shipments.Application.Common.BackgroundJobs;

public abstract class JobCommandBase : IRequest
{
    public PerformContext PerformContext { get; set; }
}