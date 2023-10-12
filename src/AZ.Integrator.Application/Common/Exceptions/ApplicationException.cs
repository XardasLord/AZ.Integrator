namespace AZ.Integrator.Application.Common.Exceptions;

public abstract class ApplicationException : Exception
{
    public abstract string Code { get; }

    protected ApplicationException()
    {
    }

    protected ApplicationException(string message) : base(message)
    {
    }
    
    protected ApplicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}