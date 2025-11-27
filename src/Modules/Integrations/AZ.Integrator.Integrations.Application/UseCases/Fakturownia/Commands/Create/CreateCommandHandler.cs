using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Application.Common;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Integrations.Domain.Aggregates.Fakturownia;
using AZ.Integrator.Integrations.Domain.Aggregates.Fakturownia.Specifications;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Fakturownia.Commands.Create;

public class CreateCommandHandler(
    IAggregateRepository<FakturowniaIntegration> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<CreateCommand, FakturowniaIntegrationViewModel>
{
    public async ValueTask<FakturowniaIntegrationViewModel> Handle(CreateCommand command, CancellationToken cancellationToken)
    {
        // Generujemy unikalny SourceSystemId dla tego tenanta
        string sourceSystemId;
        bool isUnique;
        do
        {
            sourceSystemId = SourceSystemIdGenerator.Generate("fakturownia-");
            var spec = new FakturowniaIntegrationBySourceSystemIdSpec(currentUser.TenantId, sourceSystemId);
            isUnique = !await repository.AnyAsync(spec, cancellationToken);
        } while (!isUnique);
        
        var integration = FakturowniaIntegration.Create(
            sourceSystemId,
            command.Request.ApiKey,
            command.Request.ApiUrl,
            command.Request.DisplayName,
            currentUser, currentDateTime);

        await repository.AddAsync(integration, cancellationToken);

        return new FakturowniaIntegrationViewModel
        {
            TenantId = integration.TenantId,
            SourceSystemId = integration.SourceSystemId,
            ApiKey = integration.ApiKey,
            ApiUrl = integration.ApiUrl,
            DisplayName = integration.DisplayName,
            IsEnabled = integration.IsEnabled,
            CreatedAt = integration.CreatedAt,
            UpdatedAt = integration.UpdatedAt
        };
    }
}