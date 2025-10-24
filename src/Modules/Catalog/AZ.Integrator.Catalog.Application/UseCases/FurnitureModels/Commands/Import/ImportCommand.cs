using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using Mediator;

namespace AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.Import;

public record ImportCommand(string FurnitureCode, IEnumerable<ImportPartDefinitionRequest> PartDefinitions) : ICommand;