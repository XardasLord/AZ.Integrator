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
        
        DeleteRemovedPartDefinitions(command, furnitureModel);
        AddAndUpdateParts(command, furnitureModel);

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
                Color = p.Color,
                AdditionalInfo = p.AdditionalInfo,
                FurnitureCode = furnitureModel.FurnitureCode,
                TenantId = furnitureModel.TenantId
            }).ToList()
        };
    }

    private void DeleteRemovedPartDefinitions(UpdateCommand command, FurnitureModel furnitureModel)
    {
        // Get existing part definition IDs
        var existingPartIds = furnitureModel.PartDefinitions.Select(p => p.Id).ToHashSet();
        
        // Get part definition IDs from command (only those that have IDs)
        var commandPartIds = command.PartDefinitions
            .Where(pd => pd.Id.HasValue)
            .Select(pd => pd.Id.Value)
            .ToHashSet();
        
        // Find part definitions to delete (exist in aggregate but not in command)
        var partIdsToDelete = existingPartIds.Except(commandPartIds).ToList();
        
        // Delete part definitions that are no longer present in the command
        partIdsToDelete.ForEach(id => furnitureModel.RemovePartDefinition(id, currentUser, currentDateTime));
    }

    private void AddAndUpdateParts(UpdateCommand command, FurnitureModel furnitureModel)
    {
        // Process updates and additions
        command.PartDefinitions.ToList().ForEach(pd =>
        {
            var dimensions = new Dimensions(pd.LengthMm, pd.WidthMm, pd.ThicknessMm);
            
            if (pd.Id.HasValue)
                furnitureModel.UpdatePartDefinition(
                    pd.Id.Value, pd.Name, dimensions, pd.Color, pd.AdditionalInfo, currentUser, currentDateTime);
            else
            {
                furnitureModel.AddPartDefinition(
                    pd.Name, dimensions, pd.Color, pd.AdditionalInfo, currentUser, currentDateTime);
            }
        });
    }
}