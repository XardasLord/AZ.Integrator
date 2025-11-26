using AZ.Integrator.Integrations.Contracts.RequestDtos;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Erli.Commands.Toggle;

public record ToggleCommand(string SourceSystemId, ToggleIntegrationRequest Request) : ICommand;



