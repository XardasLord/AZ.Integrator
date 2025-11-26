using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Contracts;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Fakturownia.Commands.Test;

public class TestCommandHandler(
    IIntegrationsReadFacade integrationsReadFacade,
    ICurrentUser currentUser)
    : ICommandHandler<TestCommand>
{
    public async ValueTask<Unit> Handle(TestCommand command, CancellationToken cancellationToken)
    {
        var integration = await integrationsReadFacade.GetFakturowniaIntegrationBySourceSystemIdAsync(
            currentUser.TenantId, 
            command.SourceSystemId, 
            cancellationToken);

        // TODO: Implement integration test logic
        throw new NotImplementedException("Integration test logic not implemented yet");
    }
}

