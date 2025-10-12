using AZ.Integrator.Catalog.Application.Common.Exceptions;
using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.Specifications;
using AZ.Integrator.Domain.Abstractions;
using Mediator;

namespace AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.Create;

public class CreateCommandHandler(
    IAggregateRepository<FurnitureModel> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<CreateCommand, FurnitureModelViewModel>
{
    public async ValueTask<FurnitureModelViewModel> Handle(CreateCommand command, CancellationToken cancellationToken)
    {
        var spec = new FurnitureModelByCodeSpec(command.FurnitureCode, currentUser.TenantId);
        var existingModel = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (existingModel != null)
            throw new FurnitureModelNotFoundException(command.FurnitureCode);

        var furnitureModel = FurnitureModel.Create(command.FurnitureCode, currentUser, currentDateTime);

        await repository.AddAsync(furnitureModel, cancellationToken);

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