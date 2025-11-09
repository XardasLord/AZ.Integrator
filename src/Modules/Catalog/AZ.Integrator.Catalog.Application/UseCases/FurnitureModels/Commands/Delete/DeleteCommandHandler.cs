using AZ.Integrator.Catalog.Application.Common.Exceptions;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.Specifications;
using AZ.Integrator.Domain.Abstractions;
using Mediator;

namespace AZ.Integrator.Catalog.Application.UseCases.FurnitureModels.Commands.Delete;

public class DeleteCommandHandler(
    IAggregateRepository<FurnitureModel> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime) : ICommandHandler<DeleteCommand, Unit>
{
    public async ValueTask<Unit> Handle(DeleteCommand command, CancellationToken cancellationToken)
    {
        var spec = new FurnitureModelByCodeSpec(command.FurnitureCode, currentUser.TenantId);
        var furnitureModel = await repository.FirstOrDefaultAsync(spec, cancellationToken)
            ?? throw new FurnitureModelNotFoundException(command.FurnitureCode);
        
        furnitureModel.Delete(currentDateTime);

        await repository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}