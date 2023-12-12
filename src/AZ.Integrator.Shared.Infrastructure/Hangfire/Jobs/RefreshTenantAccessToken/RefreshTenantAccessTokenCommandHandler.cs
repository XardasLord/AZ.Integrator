using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.Hangfire.Jobs.RefreshTenantAccessToken;

public class RefreshTenantAccessTokenCommandHandler : JobCommandHandlerBase<RefreshTenantAccessTokenCommand>
{
    private readonly AllegroAccountDataViewContext _allegroAccountDataViewContext;

    public RefreshTenantAccessTokenCommandHandler(AllegroAccountDataViewContext allegroAccountDataViewContext)
    {
        _allegroAccountDataViewContext = allegroAccountDataViewContext;
    }

    public override async Task<Unit> Handle(RefreshTenantAccessTokenCommand command, CancellationToken cancellationToken)
    {
        await base.Handle(command, cancellationToken);
        
        var tenantAccount = await _allegroAccountDataViewContext.AllegroAccounts
            .SingleAsync(x => x.TenantId == command.TenantId, cancellationToken);
        
        return Unit.Value;
    }
}