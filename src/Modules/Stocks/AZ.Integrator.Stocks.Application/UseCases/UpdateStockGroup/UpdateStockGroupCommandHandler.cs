using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Stocks.Application.Exceptions;
using AZ.Integrator.Stocks.Domain.Aggregates.StockGroup;
using AZ.Integrator.Stocks.Domain.Aggregates.StockGroup.Specifications;
using Mediator;

namespace AZ.Integrator.Stocks.Application.UseCases.UpdateStockGroup;

public class UpdateStockGroupCommandHandler(
    IAggregateRepository<StockGroup> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : IRequestHandler<UpdateStockGroupCommand>
{
    public async ValueTask<Unit> Handle(UpdateStockGroupCommand command, CancellationToken cancellationToken)
    {
        var spec = new StockGroupByIdSpec(command.GroupId);
        var stockGroup = await repository.SingleOrDefaultAsync(spec, cancellationToken)
            ?? throw new StockGroupNotFoundException(command.GroupId);

        stockGroup.Update(command.Name, command.Description, currentUser, currentDateTime);
        
        await repository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}