using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Application.Common.Exceptions;
using AZ.Integrator.Integrations.Domain.Aggregates.Fakturownia;
using AZ.Integrator.Integrations.Domain.Aggregates.Fakturownia.Specifications;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Fakturownia.Commands.Toggle;

public class ToggleCommandHandler(
    IAggregateRepository<FakturowniaIntegration> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<ToggleCommand>
{
    public async ValueTask<Unit> Handle(ToggleCommand command, CancellationToken cancellationToken)
    {
        var spec = new FakturowniaIntegrationBySourceSystemIdSpec(currentUser.TenantId, command.SourceSystemId);
        var integration = await repository.SingleOrDefaultAsync(spec, cancellationToken);

        if (integration == null)
            throw new IntegrationNotFoundException(currentUser.TenantId, command.SourceSystemId);

        integration.SetEnabled(command.Request.IsEnabled, currentDateTime);

        await repository.UpdateAsync(integration, cancellationToken);

        return Unit.Value;
    }
}

