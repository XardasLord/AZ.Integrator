using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using Mediator;

namespace AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.RemovePartDefinition;

public record RemovePartDefinitionCommand(string FurnitureCode, Guid PartDefinitionId) : ICommand<FurnitureModelViewModel>;