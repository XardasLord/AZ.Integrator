using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Application.Common.Exceptions;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Integrations.Domain.Aggregates.Fakturownia;
using AZ.Integrator.Integrations.Domain.Aggregates.Fakturownia.Specifications;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Fakturownia.Commands.Update;

public class UpdateCommandHandler(
    IAggregateRepository<FakturowniaIntegration> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<UpdateCommand, FakturowniaIntegrationViewModel>
{
    public async ValueTask<FakturowniaIntegrationViewModel> Handle(UpdateCommand command, CancellationToken cancellationToken)
    {
        var spec = new FakturowniaIntegrationBySourceSystemIdSpec(currentUser.TenantId, command.Request.SourceSystemId);
        var integration = await repository.SingleOrDefaultAsync(spec, cancellationToken);

        if (integration == null)
            throw new IntegrationNotFoundException(currentUser.TenantId, command.Request.SourceSystemId);

        integration.Update(
            command.Request.ApiKey,
            command.Request.ApiUrl,
            command.Request.DisplayName,
            currentDateTime);

        await repository.UpdateAsync(integration, cancellationToken);

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

