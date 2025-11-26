using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Erli.Commands.Test;

public record TestCommand(string SourceSystemId) : ICommand;