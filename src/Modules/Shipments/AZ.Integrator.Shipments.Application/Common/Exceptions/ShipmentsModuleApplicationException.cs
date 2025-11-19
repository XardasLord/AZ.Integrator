namespace AZ.Integrator.Shipments.Application.Common.Exceptions;

public abstract class ShipmentsModuleApplicationException : Exception
{
    public abstract string Code { get; }

    protected ShipmentsModuleApplicationException()
    {
    }

    protected ShipmentsModuleApplicationException(string message) : base(message)
    {
    }
    
    protected ShipmentsModuleApplicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}