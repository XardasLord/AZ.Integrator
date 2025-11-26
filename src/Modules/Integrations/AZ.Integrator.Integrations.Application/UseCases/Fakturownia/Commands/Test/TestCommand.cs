using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Fakturownia.Commands.Test;

public record TestCommand(string SourceSystemId) : ICommand;

