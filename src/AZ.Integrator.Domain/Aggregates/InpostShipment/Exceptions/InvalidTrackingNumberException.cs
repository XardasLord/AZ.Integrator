using AZ.Integrator.Domain.SharedKernel.Exceptions;

namespace AZ.Integrator.Domain.Aggregates.InpostShipment.Exceptions;

public class InvalidTrackingNumberException : DomainException
{
    public string TrackingNumber { get; }
    public override string Code => "invalid_tracking_number";

    public InvalidTrackingNumberException(string trackingNumber, string message) : base(message)
    {
        TrackingNumber = trackingNumber;
    }
}