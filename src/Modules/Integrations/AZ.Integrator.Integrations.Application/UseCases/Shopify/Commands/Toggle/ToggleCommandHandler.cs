using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Application.Common.Exceptions;
using AZ.Integrator.Integrations.Domain.Aggregates.Shopify;
using AZ.Integrator.Integrations.Domain.Aggregates.Shopify.Specifications;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Shopify.Commands.Toggle;

public class ToggleCommandHandler(
    IAggregateRepository<ShopifyIntegration> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<ToggleCommand>
{
    public async ValueTask<Unit> Handle(ToggleCommand command, CancellationToken cancellationToken)
    {
        var spec = new ShopifyIntegrationBySourceSystemIdSpec(currentUser.TenantId, command.SourceSystemId);
        var integration = await repository.SingleOrDefaultAsync(spec, cancellationToken);

        if (integration == null)
            throw new IntegrationNotFoundException(currentUser.TenantId, command.SourceSystemId);

        integration.SetEnabled(command.Request.IsEnabled, currentDateTime);

        await repository.UpdateAsync(integration, cancellationToken);

        return Unit.Value;
    }
}

