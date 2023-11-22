using AZ.Integrator.Shared.Infrastructure.Exceptions;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.SubiektGT.Exceptions;

public class SubiektOperationException : InfrastructureException
{
    public override string Code => "subiekt_operation_exception";

    public SubiektOperationException()
    {
    }

    public SubiektOperationException(string message) : base(message)
    {
    }

    public SubiektOperationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}