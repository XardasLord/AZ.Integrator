using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Application.Common.Exceptions;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Integrations.Domain.Aggregates.Shopify;
using AZ.Integrator.Integrations.Domain.Aggregates.Shopify.Specifications;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Shopify.Commands.Update;

public class UpdateCommandHandler(
    IAggregateRepository<ShopifyIntegration> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<UpdateCommand, ShopifyIntegrationViewModel>
{
    public async ValueTask<ShopifyIntegrationViewModel> Handle(UpdateCommand command, CancellationToken cancellationToken)
    {
        var spec = new ShopifyIntegrationBySourceSystemIdSpec(currentUser.TenantId, command.Request.SourceSystemId);
        var integration = await repository.SingleOrDefaultAsync(spec, cancellationToken);

        if (integration == null)
            throw new IntegrationNotFoundException(currentUser.TenantId, command.Request.SourceSystemId);

        integration.Update(
            command.Request.ApiUrl,
            command.Request.AdminApiToken,
            command.Request.DisplayName,
            currentDateTime);

        await repository.UpdateAsync(integration, cancellationToken);

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

