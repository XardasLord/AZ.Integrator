using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Fakturownia.Commands.Delete;

public record DeleteCommand(string SourceSystemId) : ICommand;

