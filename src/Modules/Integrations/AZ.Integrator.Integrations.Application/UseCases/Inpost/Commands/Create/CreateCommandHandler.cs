using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Application.Common.Exceptions;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Integrations.Domain.Aggregates.Inpost;
using AZ.Integrator.Integrations.Domain.Aggregates.Inpost.Specifications;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Inpost.Commands.Create;

public class CreateCommandHandler(
    IAggregateRepository<InpostIntegration> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<CreateCommand, InpostIntegrationViewModel>
{
    public async ValueTask<InpostIntegrationViewModel> Handle(CreateCommand command, CancellationToken cancellationToken)
    {
        var spec = new InpostIntegrationByOrganizationIdSpec(currentUser.TenantId, command.Request.OrganizationId);
        var exists = await repository.AnyAsync(spec, cancellationToken);

        if (exists)
            throw new IntegrationAlreadyExistsException(currentUser.TenantId, command.Request.OrganizationId.ToString());
        
        var integration = InpostIntegration.Create(
            command.Request.OrganizationId,
            command.Request.AccessToken,
            command.Request.DisplayName,
            currentUser, currentDateTime);

        await repository.AddAsync(integration, cancellationToken);

        return new InpostIntegrationViewModel
        {
            TenantId = integration.TenantId,
            OrganizationId = integration.OrganizationId,
            AccessToken = integration.AccessToken,
            DisplayName = integration.DisplayName,
            IsEnabled = integration.IsEnabled,
            CreatedAt = integration.CreatedAt,
            UpdatedAt = integration.UpdatedAt
        };
    }
}