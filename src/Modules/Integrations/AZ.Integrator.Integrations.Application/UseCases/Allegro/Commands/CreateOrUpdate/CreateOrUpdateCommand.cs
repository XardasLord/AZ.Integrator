using AZ.Integrator.Integrations.Contracts;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Allegro.Commands.CreateOrUpdate;

public record CreateOrUpdateCommand(Guid TenantId, AllegroIntegrationCreateModel Request) : ICommand<AllegroIntegrationViewModel>;