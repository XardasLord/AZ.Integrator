using AZ.Integrator.Shared.Infrastructure.Hangfire.Jobs;
using Mediator;

namespace AZ.Integrator.Integrations.Infrastructure.Hangfire.Jobs.RefreshTenantAccessToken;

public class RefreshAllegroTenantAccessTokenCommandJob(IMediator mediator) : JobBase<RefreshAllegroTenantAccessTokenCommand>(mediator);