using AZ.Integrator.Catalog.Application.Common.Exceptions;
using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.Specifications;
using AZ.Integrator.Domain.Abstractions;
using Mediator;

namespace AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.RemovePartDefinition;

public class RemovePartDefinitionCommandHandler(
    IAggregateRepository<FurnitureModel> repository,
    ICurrentUser currentUser) : ICommandHandler<RemovePartDefinitionCommand, FurnitureModelViewModel>
{
    public async ValueTask<FurnitureModelViewModel> Handle(RemovePartDefinitionCommand command, CancellationToken cancellationToken)
    {
        var spec = new FurnitureModelByCodeSpec(command.FurnitureCode, currentUser.TenantId);
        var furnitureModel = await repository.FirstOrDefaultAsync(spec, cancellationToken)
                             ?? throw new FurnitureModelNotFoundException(command.FurnitureCode);

        furnitureModel.RemovePartDefinition(command.PartDefinitionId);

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