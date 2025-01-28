namespace AZ.Integrator.Stocks.Application.Exceptions;

public class StockNotFoundException(string packageCode)
    : StocksModuleApplicationException($"Stock with package code {packageCode} was not found.")
{
    public override string Code => "stock_not_found";
}