using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Inpost.Commands.Test;

public record TestCommand(int OrganizationId) : ICommand;

