namespace AZ.Integrator.Integrations.Application.Common.Exceptions;

public abstract class IntegrationsModuleApplicationException : Exception
{
    public abstract string Code { get; }

    protected IntegrationsModuleApplicationException()
    {
    }

    protected IntegrationsModuleApplicationException(string message) : base(message)
    {
    }
    
    protected IntegrationsModuleApplicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}