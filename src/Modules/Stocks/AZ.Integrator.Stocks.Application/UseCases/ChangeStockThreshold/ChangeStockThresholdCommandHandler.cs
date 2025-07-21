using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Stocks.Application.Exceptions;
using AZ.Integrator.Stocks.Domain.Aggregates.Stock;
using AZ.Integrator.Stocks.Domain.Aggregates.Stock.Specifications;
using Mediator;

namespace AZ.Integrator.Stocks.Application.UseCases.ChangeStockThreshold;

public class ChangeStockThresholdCommandHandler(IAggregateRepository<Stock> repository) : IRequestHandler<ChangeStockThresholdCommand>
{
    public async ValueTask<Unit> Handle(ChangeStockThresholdCommand command, CancellationToken cancellationToken)
    {
        var spec = new StockByPackageCodeSpec(command.PackageCode);
        var stock = await repository.SingleOrDefaultAsync(spec, cancellationToken)
            ?? throw new StockNotFoundException(command.PackageCode);

        stock.ChangeThreshold(command.Threshold);
        await repository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}