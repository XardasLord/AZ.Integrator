using AZ.Integrator.Integrations.Application.UseCases.Allegro.Commands.CreateOrUpdate;
using AZ.Integrator.Integrations.Contracts;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using Mediator;

namespace AZ.Integrator.Integrations.Infrastructure.Facade;

public class IntegrationsWriteFacade(IMediator mediator) : IIntegrationsWriteFacade
{
    public async Task<AllegroIntegrationViewModel> AddAllegroIntegrationAsync(Guid tenantId, AllegroIntegrationCreateModel createModel, CancellationToken cancellationToken = default)
    {
        var command = new CreateOrUpdateCommand(tenantId, createModel);
        
        var result = await mediator.Send(command, cancellationToken);
        
        return result;
    }
}