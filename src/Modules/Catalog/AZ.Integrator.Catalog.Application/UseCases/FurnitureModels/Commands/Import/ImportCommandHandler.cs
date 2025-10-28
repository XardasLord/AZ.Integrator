using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.Specifications;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;
using AZ.Integrator.Domain.Abstractions;
using Mediator;

namespace AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.Import;

public class ImportCommandHandler(
    IAggregateRepository<FurnitureModel> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<ImportCommand>
{
    public async ValueTask<Unit> Handle(ImportCommand command, CancellationToken cancellationToken)
    {
        var spec = new FurnitureModelByCodeSpec(command.FurnitureCode, currentUser.TenantId);
        var aggregateModel = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        
        var partDefinitionVos = new List<PartDefinitionVo>();

        command.PartDefinitions.ToList().ForEach(pd =>
        {
            var partDefinitionVo = new PartDefinitionVo(null, pd.Name,
                new Dimensions(pd.LengthMm, pd.WidthMm, pd.ThicknessMm, (EdgeBandingType)pd.LengthEdgeBandingType, (EdgeBandingType)pd.WidthEdgeBandingType), 
                pd.Quantity, pd.AdditionalInfo);

            partDefinitionVos.Add(partDefinitionVo);
        });

        if (aggregateModel is null)
        {
            var furnitureModel =
                FurnitureModel.Create(command.FurnitureCode, partDefinitionVos, currentUser, currentDateTime);
            
            await repository.AddAsync(furnitureModel, cancellationToken);
        }
        else
        {
            aggregateModel.UpdatePartDefinitions(partDefinitionVos, currentUser, currentDateTime);

            await repository.SaveChangesAsync(cancellationToken);
        }
        
        return Unit.Value;
    }
}