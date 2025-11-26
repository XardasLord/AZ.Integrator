using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Application.Common.Exceptions;
using AZ.Integrator.Integrations.Domain.Aggregates.Inpost;
using AZ.Integrator.Integrations.Domain.Aggregates.Inpost.Specifications;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Inpost.Commands.Delete;

public class DeleteCommandHandler(
    IAggregateRepository<InpostIntegration> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<DeleteCommand>
{
    public async ValueTask<Unit> Handle(DeleteCommand command, CancellationToken cancellationToken)
    {
        var spec = new InpostIntegrationByOrganizationIdSpec(currentUser.TenantId, command.OrganizationId);
        var integration = await repository.SingleOrDefaultAsync(spec, cancellationToken);

        if (integration == null)
            throw new IntegrationNotFoundException(currentUser.TenantId, command.OrganizationId.ToString());

        integration.Delete(currentUser, currentDateTime);

        await repository.UpdateAsync(integration, cancellationToken);

        return Unit.Value;
    }
}

