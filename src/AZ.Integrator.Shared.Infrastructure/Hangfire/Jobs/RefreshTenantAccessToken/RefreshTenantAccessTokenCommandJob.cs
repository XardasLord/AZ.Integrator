using MediatR;

namespace AZ.Integrator.Shared.Infrastructure.Hangfire.Jobs.RefreshTenantAccessToken;

public class RefreshTenantAccessTokenCommandJob : JobBase<RefreshTenantAccessTokenCommand>
{
    public RefreshTenantAccessTokenCommandJob(IMediator mediator) : base(mediator)
    {
    }
}