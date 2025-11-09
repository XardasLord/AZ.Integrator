using Mediator;

namespace AZ.Integrator.Shared.Infrastructure.Hangfire.Jobs.RefreshTenantAccessToken;

public class RefreshTenantAccessTokenCommandJob(IMediator mediator) : JobBase<RefreshTenantAccessTokenCommand>(mediator);