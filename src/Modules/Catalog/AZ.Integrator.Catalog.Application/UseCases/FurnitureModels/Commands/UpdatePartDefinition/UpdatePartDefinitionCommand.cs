using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using Mediator;

namespace AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.UpdatePartDefinition;

public record UpdatePartDefinitionCommand(
    string FurnitureCode,
    Guid PartDefinitionId,
    string Name,
    int LengthMm,
    int WidthMm,
    int ThicknessMm,
    string Color,
    string AdditionalInfo
) : ICommand<FurnitureModelViewModel>;