using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Shopify.Commands.Delete;

public record DeleteCommand(string SourceSystemId) : ICommand;