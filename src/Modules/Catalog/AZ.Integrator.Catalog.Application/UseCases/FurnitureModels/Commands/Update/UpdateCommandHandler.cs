using AZ.Integrator.Catalog.Application.Common.Exceptions;
using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.Specifications;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;
using AZ.Integrator.Domain.Abstractions;
using Mediator;

namespace AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.Update;

public class UpdateCommandHandler(
    IAggregateRepository<FurnitureModel> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<UpdateCommand, FurnitureModelViewModel>
{
    public async ValueTask<FurnitureModelViewModel> Handle(UpdateCommand command, CancellationToken cancellationToken)
    {
        var spec = new FurnitureModelByCodeSpec(command.FurnitureCode, currentUser.TenantId);
        var furnitureModel = await repository.FirstOrDefaultAsync(spec, cancellationToken)
            ?? throw new FurnitureModelNotFoundException(command.FurnitureCode);
        
        var partDefinitionVos = new List<PartDefinitionVo>();

        command.PartDefinitions.ToList().ForEach(pd =>
        {
            var partDefinitionVo = new PartDefinitionVo(pd.Id, pd.Name,
                new Dimensions(pd.LengthMm, pd.WidthMm, pd.ThicknessMm), 
                pd.Quantity, pd.AdditionalInfo);

            partDefinitionVos.Add(partDefinitionVo);
        });
        
        furnitureModel.UpdatePartDefinitions(partDefinitionVos, currentUser, currentDateTime);

        await repository.SaveChangesAsync(cancellationToken);

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