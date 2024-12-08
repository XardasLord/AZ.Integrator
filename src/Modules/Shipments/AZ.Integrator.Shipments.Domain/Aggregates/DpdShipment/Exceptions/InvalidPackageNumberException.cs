using AZ.Integrator.Domain.SharedKernel.Exceptions;

namespace AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.Exceptions;

public class InvalidPackageNumberException : DomainException
{
    public long PackageNumber { get; }
    public override string Code => "invalid_package_number";

    public InvalidPackageNumberException(long packageNumber, string message) : base(message)
    {
        PackageNumber = packageNumber;
    }
}