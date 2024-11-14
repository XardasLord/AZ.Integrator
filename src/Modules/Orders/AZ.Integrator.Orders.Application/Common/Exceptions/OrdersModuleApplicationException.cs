namespace AZ.Integrator.Orders.Application.Common.Exceptions;

public abstract class OrdersModuleApplicationException : Exception
{
    public abstract string Code { get; }

    protected OrdersModuleApplicationException()
    {
    }

    protected OrdersModuleApplicationException(string message) : base(message)
    {
    }
    
    protected OrdersModuleApplicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}