using AZ.Integrator.Domain.SharedKernel.Exceptions;

namespace AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.Exceptions;

public class InvalidParcelNumberException : DomainException
{
    public long ParcelNumber { get; }
    public override string Code => "invalid_parcel_number";

    public InvalidParcelNumberException(long parcelNumber, string message) : base(message)
    {
        ParcelNumber = parcelNumber;
    }
}