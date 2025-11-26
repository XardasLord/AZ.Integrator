using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Shopify.Commands.Test;

public record TestCommand(string SourceSystemId) : ICommand;