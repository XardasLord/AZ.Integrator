using AZ.Integrator.Integrations.Contracts.RequestDtos;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Inpost.Commands.Toggle;

public record ToggleCommand(int OrganizationId, ToggleIntegrationRequest Request) : ICommand;

