using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using Mediator;

namespace AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.Create;

public record CreateCommand(string FurnitureCode, IEnumerable<AddPartDefinitionRequest> PartDefinitions) : ICommand<FurnitureModelViewModel>;