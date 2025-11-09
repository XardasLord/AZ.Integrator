using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using Mediator;

namespace AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.Update;

public record UpdateCommand(string FurnitureCode, IEnumerable<UpdatePartDefinitionRequest> PartDefinitions) : ICommand<FurnitureModelViewModel>;