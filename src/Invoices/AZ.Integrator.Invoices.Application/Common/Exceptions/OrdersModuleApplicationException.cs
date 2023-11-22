namespace AZ.Integrator.Invoices.Application.Common.Exceptions;

public abstract class InvoicesModuleApplicationException : Exception
{
    public abstract string Code { get; }

    protected InvoicesModuleApplicationException()
    {
    }

    protected InvoicesModuleApplicationException(string message) : base(message)
    {
    }
    
    protected InvoicesModuleApplicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}