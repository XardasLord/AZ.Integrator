using AZ.Integrator.Catalog.Application.Common.Exceptions;
using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.Specifications;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;
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
        
        var partDefinitionVos = new List<PartDefinitionVo>();

        command.PartDefinitions.ToList().ForEach(pd =>
        {
            var partDefinitionVo = new PartDefinitionVo(null, pd.Name,
                new Dimensions(pd.LengthMm, pd.WidthMm, pd.ThicknessMm), 
                pd.Quantity, pd.AdditionalInfo);

            partDefinitionVos.Add(partDefinitionVo);
        });

        var furnitureModel = FurnitureModel.Create(command.FurnitureCode, partDefinitionVos, currentUser, currentDateTime);

        await repository.AddAsync(furnitureModel, cancellationToken);

        return new FurnitureModelViewModel
        {
            FurnitureCode = furnitureModel.FurnitureCode,
            TenantId = furnitureModel.TenantId,
            Version = furnitureModel.Version,
            IsDeleted = furnitureModel.IsDeleted,
            DeletedAt = furnitureModel.DeletedAt,
            CreatedBy = furnitureModel.CreationInformation.CreatedBy,
            CreatedAt = furnitureModel.CreationInformation.CreatedAt.Date,
            PartDefinitions = furnitureModel.PartDefinitions.Select(p => new PartDefinitionViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Dimensions = new DimensionsViewModel
                {
                    LengthMm = p.Dimensions.LengthMm,
                    WidthMm = p.Dimensions.WidthMm,
                    ThicknessMm = p.Dimensions.ThicknessMm
                },
                Quantity = p.Quantity.Value,
                AdditionalInfo = p.AdditionalInfo,
                FurnitureCode = furnitureModel.FurnitureCode,
                TenantId = furnitureModel.TenantId
            }).ToList()
        };
    }
}