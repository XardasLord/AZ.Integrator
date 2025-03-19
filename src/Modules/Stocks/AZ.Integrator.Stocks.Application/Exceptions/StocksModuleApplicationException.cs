namespace AZ.Integrator.Stocks.Application.Exceptions;

public abstract class StocksModuleApplicationException : Exception
{
    public abstract string Code { get; }

    protected StocksModuleApplicationException()
    {
    }

    protected StocksModuleApplicationException(string message) : base(message)
    {
    }
    
    protected StocksModuleApplicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}