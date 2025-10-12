using Mediator;

namespace AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.Delete;

public record DeleteCommand(string FurnitureCode) : ICommand<Unit>;