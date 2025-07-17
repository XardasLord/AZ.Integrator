namespace AZ.Integrator.Stocks.Application.Exceptions;

public class StockGroupNotFoundException(int groupId)
    : StocksModuleApplicationException($"Stock group with ID {groupId} was not found.")
{
    public override string Code => "stock_group_not_found";
}