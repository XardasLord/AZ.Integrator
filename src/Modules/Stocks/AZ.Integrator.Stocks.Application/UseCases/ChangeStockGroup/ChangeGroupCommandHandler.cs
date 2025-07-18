using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Stocks.Application.Exceptions;
using AZ.Integrator.Stocks.Domain.Aggregates.Stock;
using AZ.Integrator.Stocks.Domain.Aggregates.Stock.Specifications;
using Mediator;

namespace AZ.Integrator.Stocks.Application.UseCases.ChangeStockGroup;

public class ChangeStockGroupCommandHandler(IAggregateRepository<Stock> repository) : IRequestHandler<ChangeStockGroupCommand>
{
    public async ValueTask<Unit> Handle(ChangeStockGroupCommand command, CancellationToken cancellationToken)
    {
        var spec = new StockByPackageCodeSpec(command.PackageCode);
        var stock = await repository.SingleOrDefaultAsync(spec, cancellationToken)
            ?? throw new StockNotFoundException(command.PackageCode);

        stock.AssignToGroup(command.NewGroupId);
        await repository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}