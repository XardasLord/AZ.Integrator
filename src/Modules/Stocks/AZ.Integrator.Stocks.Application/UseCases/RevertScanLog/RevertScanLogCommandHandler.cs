using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Stocks.Application.Exceptions;
using AZ.Integrator.Stocks.Domain.Aggregates;
using AZ.Integrator.Stocks.Domain.Aggregates.Specifications;
using Mediator;

namespace AZ.Integrator.Stocks.Application.UseCases.RevertScanLog;

public class RevertScanLogCommandHandler(
    IAggregateRepository<Stock> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : IRequestHandler<RevertScanLogCommand>
{
    public async ValueTask<Unit> Handle(RevertScanLogCommand command, CancellationToken cancellationToken)
    {
        var spec = new StockByPackageCodeSpec(command.PackageCode);
        var stock = await repository.SingleOrDefaultAsync(spec, cancellationToken);

        if (stock is null)
            throw new StockNotFoundException(command.PackageCode);

        stock.RevertScannedLog((uint)command.ScanLogId, currentUser, currentDateTime);
        
        await repository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}