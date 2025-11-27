using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Application.Common;
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
        // Generujemy unikalny SourceSystemId dla tego tenanta
        string sourceSystemId;
        bool isUnique;
        do
        {
            sourceSystemId = SourceSystemIdGenerator.Generate("shopify-");
            var spec = new ShopifyIntegrationBySourceSystemIdSpec(currentUser.TenantId, sourceSystemId);
            isUnique = !await repository.AnyAsync(spec, cancellationToken);
        } while (!isUnique);
        
        var integration = ShopifyIntegration.Create(
            sourceSystemId,
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