using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Stocks.Domain.Aggregates.StockGroup;
using Mediator;

namespace AZ.Integrator.Stocks.Application.UseCases.AddStockGroup;

public class AddStockGroupCommandHandler(
    IAggregateRepository<StockGroup> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : IRequestHandler<AddStockGroupCommand, uint>
{
    public async ValueTask<uint> Handle(AddStockGroupCommand command, CancellationToken cancellationToken)
    {
        var stockGroup = StockGroup.Create(command.Name, command.Description, currentUser, currentDateTime);
        
        await repository.AddAsync(stockGroup, cancellationToken);
        
        return stockGroup.Id;
    }
}