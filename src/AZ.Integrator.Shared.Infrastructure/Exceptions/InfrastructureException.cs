namespace AZ.Integrator.Shared.Infrastructure.Exceptions;

public abstract class InfrastructureException : Exception
{
    public abstract string Code { get; }

    protected InfrastructureException()
    {
    }

    protected InfrastructureException(string message) : base(message)
    {
    }
    
    protected InfrastructureException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}