using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Inpost.Commands.Delete;

public record DeleteCommand(int OrganizationId) : ICommand;

