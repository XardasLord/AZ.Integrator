using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Stocks.Application.Exceptions;
using AZ.Integrator.Stocks.Domain.Aggregates.Stock;
using AZ.Integrator.Stocks.Domain.Aggregates.Stock.Specifications;
using Mediator;

namespace AZ.Integrator.Stocks.Application.UseCases.ChangeQuantity;

public class ChangeQuantityCommandHandler(
    IAggregateRepository<Stock> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : IRequestHandler<ChangeQuantityCommand>
{
    public async ValueTask<Unit> Handle(ChangeQuantityCommand command, CancellationToken cancellationToken)
    {
        var spec = new StockByPackageCodeSpec(command.PackageCode);
        var stock = await repository.SingleOrDefaultAsync(spec, cancellationToken);

        if (stock is null)
        {
            if (command.ChangeQuantity < 0)
            {
                throw new StockNotFoundException(command.PackageCode);
            }

            stock = Stock.Register(command.PackageCode, command.ChangeQuantity, command.ScanId, currentUser, currentDateTime);
            await repository.AddAsync(stock, cancellationToken);
        }
        else
        {
            stock.UpdateQuantity(command.ChangeQuantity, command.ScanId, currentUser, currentDateTime);
            await repository.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}