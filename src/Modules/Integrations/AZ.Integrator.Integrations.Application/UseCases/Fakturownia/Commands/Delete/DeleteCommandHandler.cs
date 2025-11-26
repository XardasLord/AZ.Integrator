using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Application.Common.Exceptions;
using AZ.Integrator.Integrations.Domain.Aggregates.Fakturownia;
using AZ.Integrator.Integrations.Domain.Aggregates.Fakturownia.Specifications;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Fakturownia.Commands.Delete;

public class DeleteCommandHandler(
    IAggregateRepository<FakturowniaIntegration> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<DeleteCommand>
{
    public async ValueTask<Unit> Handle(DeleteCommand command, CancellationToken cancellationToken)
    {
        var spec = new FakturowniaIntegrationBySourceSystemIdSpec(currentUser.TenantId, command.SourceSystemId);
        var integration = await repository.SingleOrDefaultAsync(spec, cancellationToken);

        if (integration == null)
            throw new IntegrationNotFoundException(currentUser.TenantId, command.SourceSystemId);

        integration.Delete(currentUser, currentDateTime);

        await repository.UpdateAsync(integration, cancellationToken);

        return Unit.Value;
    }
}

