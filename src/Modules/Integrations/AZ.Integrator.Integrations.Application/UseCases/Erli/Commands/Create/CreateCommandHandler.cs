using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Application.Common;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Integrations.Domain.Aggregates.Erli;
using AZ.Integrator.Integrations.Domain.Aggregates.Erli.Specifications;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Erli.Commands.Create;

public class CreateCommandHandler(
    IAggregateRepository<ErliIntegration> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<CreateCommand, ErliIntegrationViewModel>
{
    public async ValueTask<ErliIntegrationViewModel> Handle(CreateCommand command, CancellationToken cancellationToken)
    {
        // Generujemy unikalny SourceSystemId dla tego tenanta
        string sourceSystemId;
        bool isUnique;
        do
        {
            sourceSystemId = SourceSystemIdGenerator.Generate("erli-");
            var spec = new ErliIntegrationBySourceSystemIdSpec(currentUser.TenantId, sourceSystemId);
            isUnique = !await repository.AnyAsync(spec, cancellationToken);
        } while (!isUnique);
        
        var integration = ErliIntegration.Create(
            sourceSystemId,
            command.Request.ApiKey,
            command.Request.DisplayName,
            currentUser, currentDateTime);

        await repository.AddAsync(integration, cancellationToken);

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