using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Erli.Commands.Delete;

public record DeleteCommand(string SourceSystemId) : ICommand;

