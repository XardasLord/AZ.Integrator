using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Application.Common.Exceptions;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Integrations.Domain.Aggregates.Shopify;
using AZ.Integrator.Integrations.Domain.Aggregates.Shopify.Specifications;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Shopify.Commands.Create;

public class CreateCommandHandler(
    IAggregateRepository<ShopifyIntegration> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<CreateCommand, ShopifyIntegrationViewModel>
{
    public async ValueTask<ShopifyIntegrationViewModel> Handle(CreateCommand command, CancellationToken cancellationToken)
    {
        var spec = new ShopifyIntegrationBySourceSystemIdSpec(currentUser.TenantId, command.Request.SourceSystemId);
        var exists = await repository.AnyAsync(spec, cancellationToken);

        if (exists)
            throw new IntegrationAlreadyExistsException(currentUser.TenantId, command.Request.SourceSystemId);
        
        var integration = ShopifyIntegration.Create(
            command.Request.SourceSystemId,
            command.Request.ApiUrl,
            command.Request.AdminApiToken,
            command.Request.DisplayName,
            currentUser, currentDateTime);

        await repository.AddAsync(integration, cancellationToken);

        return new ShopifyIntegrationViewModel
        {
            TenantId = integration.TenantId,
            SourceSystemId = integration.SourceSystemId,
            ApiUrl = integration.ApiUrl,
            AdminApiToken = integration.AdminApiToken,
            DisplayName = integration.DisplayName,
            IsEnabled = integration.IsEnabled,
            CreatedAt = integration.CreatedAt,
            UpdatedAt = integration.UpdatedAt
        };
    }
}