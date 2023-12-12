using Hangfire.Server;
using MediatR;

namespace AZ.Integrator.Shared.Infrastructure.Hangfire.Jobs;

public abstract class JobCommandBase : IRequest
{
    public PerformContext PerformContext { get; set; }
}