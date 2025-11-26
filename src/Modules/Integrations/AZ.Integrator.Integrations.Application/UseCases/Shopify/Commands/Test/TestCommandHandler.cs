using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Contracts;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Shopify.Commands.Test;

public class TestCommandHandler(
    IIntegrationsReadFacade integrationsReadFacade,
    ICurrentUser currentUser)
    : ICommandHandler<Erli.Commands.Test.TestCommand>
{
    public async ValueTask<Unit> Handle(Erli.Commands.Test.TestCommand command, CancellationToken cancellationToken)
    {
        var integration = await integrationsReadFacade.GetShopifyIntegrationBySourceSystemIdAsync(
            currentUser.TenantId, 
            command.SourceSystemId, 
            cancellationToken);

        // TODO: Implement integration test logic
        throw new NotImplementedException("Integration test logic not implemented yet");
    }
}

