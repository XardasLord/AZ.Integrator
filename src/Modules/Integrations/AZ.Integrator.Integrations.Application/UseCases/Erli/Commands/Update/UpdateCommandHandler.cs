using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Application.Common.Exceptions;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Integrations.Domain.Aggregates.Erli;
using AZ.Integrator.Integrations.Domain.Aggregates.Erli.Specifications;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Erli.Commands.Update;

public class UpdateCommandHandler(
    IAggregateRepository<ErliIntegration> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<UpdateCommand, ErliIntegrationViewModel>
{
    public async ValueTask<ErliIntegrationViewModel> Handle(UpdateCommand command, CancellationToken cancellationToken)
    {
        var spec = new ErliIntegrationBySourceSystemIdSpec(currentUser.TenantId, command.Request.SourceSystemId);
        var integration = await repository.SingleOrDefaultAsync(spec, cancellationToken);

        if (integration == null)
            throw new IntegrationNotFoundException(currentUser.TenantId, command.Request.SourceSystemId);

        integration.Update(
            command.Request.ApiKey,
            command.Request.DisplayName,
            currentDateTime);

        await repository.UpdateAsync(integration, cancellationToken);

        return new ErliIntegrationViewModel
        {
            TenantId = integration.TenantId,
            SourceSystemId = integration.SourceSystemId,
            ApiKey = integration.ApiKey,
            DisplayName = integration.DisplayName,
            IsEnabled = integration.IsEnabled,
            CreatedAt = integration.CreatedAt,
            UpdatedAt = integration.UpdatedAt
        };
    }
}

