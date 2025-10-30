namespace AZ.Integrator.Procurement.Application.Common.Exceptions;

public abstract class ProcurementModuleApplicationException : Exception
{
    public abstract string Code { get; }

    protected ProcurementModuleApplicationException()
    {
    }

    protected ProcurementModuleApplicationException(string message) : base(message)
    {
    }
    
    protected ProcurementModuleApplicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}