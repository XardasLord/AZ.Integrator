using AZ.Integrator.Domain.SharedKernel.Exceptions;

namespace AZ.Integrator.Domain.Aggregates.DpdShipment.Exceptions;

public class InvalidSessionNumberException : DomainException
{
    public long SessionNumber { get; }
    public override string Code => "invalid_session_number";

    public InvalidSessionNumberException(long sessionNumber, string message) : base(message)
    {
        SessionNumber = sessionNumber;
    }
}