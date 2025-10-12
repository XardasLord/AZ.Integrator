using AZ.Integrator.Catalog.Application.Common.Exceptions;
using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.Specifications;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using Mediator;

namespace AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.UpdatePartDefinition;

public class UpdatePartDefinitionCommandHandler(
    IAggregateRepository<FurnitureModel> repository,
    ICurrentUser currentUser) : ICommandHandler<UpdatePartDefinitionCommand, FurnitureModelViewModel>
{
    public async ValueTask<FurnitureModelViewModel> Handle(UpdatePartDefinitionCommand command, CancellationToken cancellationToken)
    {
        var spec = new FurnitureModelByCodeSpec(command.FurnitureCode, currentUser.TenantId);
        var furnitureModel = await repository.FirstOrDefaultAsync(spec, cancellationToken)
                             ?? throw new FurnitureModelNotFoundException(command.FurnitureCode);

        var partName = PartName.Create(command.Name);
        var dimensions = Dimensions.Create(command.LengthMm, command.WidthMm, command.ThicknessMm);
        var color = Color.Create(command.Color);

        furnitureModel.UpdatePartDefinition(command.PartDefinitionId, partName, dimensions, color);

        await repository.SaveChangesAsync(cancellationToken);

        return new FurnitureModelViewModel(
            furnitureModel.FurnitureCode,
            furnitureModel.TenantId,
            furnitureModel.Version,
            furnitureModel.IsDeleted,
            furnitureModel.DeletedAt,
            furnitureModel.CreationInformation.CreatedBy,
            furnitureModel.CreationInformation.CreatedAt.Date,
            furnitureModel.PartDefinitions.Select(p => new PartDefinitionViewModel(
                p.Id,
                p.Name,
                new DimensionsViewModel(p.Dimensions.LengthMm, p.Dimensions.WidthMm, p.Dimensions.ThicknessMm),
                p.Color,
                p.AdditionalInfo
            )).ToList()
        );
    }
}