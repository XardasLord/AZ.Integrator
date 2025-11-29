using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Integrations.Domain.Aggregates.Allegro;
using AZ.Integrator.Integrations.Domain.Aggregates.Allegro.Specifications;
using AZ.Integrator.Orders.Contracts;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Allegro.Commands.CreateOrUpdate;

public class CreateCommandHandler(
    IAggregateRepository<AllegroIntegration> repository,
    IOrdersAllegroIntegrationFacade ordersAllegroIntegrationService,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<CreateOrUpdateCommand, AllegroIntegrationViewModel>
{
    public async ValueTask<AllegroIntegrationViewModel> Handle(CreateOrUpdateCommand command, CancellationToken cancellationToken)
    {
        var accountDetails = await ordersAllegroIntegrationService.GetShopInfo(command.TenantId, command.Request.AccessToken, cancellationToken) 
                             ?? throw new InvalidOperationException("Nie udało się pobrać informacji o sklepie Allegro.");

        var sourceSystemId = $"allegro-{accountDetails.Id}";
        
        var existingIntegrationSpec = new AllegroIntegrationBySourceSystemIdSpec(command.TenantId, sourceSystemId);
        var integration = await repository.FirstOrDefaultAsync(existingIntegrationSpec, cancellationToken);
        
        if (integration is not null)
        {
            integration.UpdateTokens(
                command.Request.AccessToken,
                command.Request.RefreshToken,
                command.Request.ExpiresAt,
                currentDateTime);

            await repository.SaveChangesAsync(cancellationToken);
        }
        else
        {
            integration = AllegroIntegration.Create(
                sourceSystemId,
                command.TenantId,
                command.Request.AccessToken,
                command.Request.RefreshToken,
                command.Request.ExpiresAt,
                accountDetails.Company?.Name ?? accountDetails.Login,
                currentUser, currentDateTime);

            await repository.AddAsync(integration, cancellationToken);
        }

        return new AllegroIntegrationViewModel
        {
            TenantId = integration.TenantId,
            SourceSystemId = integration.SourceSystemId,
            AccessToken = integration.AccessToken,
            RefreshToken = integration.RefreshToken,
            ExpiresAt = integration.ExpiresAt,
            DisplayName = integration.DisplayName,
            IsEnabled = integration.IsEnabled,
            CreatedAt = integration.CreatedAt,
            UpdatedAt = integration.UpdatedAt
        };
    }
}