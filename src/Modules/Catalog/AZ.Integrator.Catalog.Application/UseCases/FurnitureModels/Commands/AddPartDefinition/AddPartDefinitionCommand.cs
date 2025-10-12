using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using Mediator;

namespace AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.AddPartDefinition;

public record AddPartDefinitionCommand(
    string FurnitureCode,
    string Name,
    int LengthMm,
    int WidthMm,
    int ThicknessMm,
    string Color,
    string AdditionalComment
) : ICommand<FurnitureModelViewModel>;