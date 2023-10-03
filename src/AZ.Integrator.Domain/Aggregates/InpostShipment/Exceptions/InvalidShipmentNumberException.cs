using AZ.Integrator.Domain.SharedKernel.Exceptions;

namespace AZ.Integrator.Domain.Aggregates.InpostShipment.Exceptions;

public class InvalidShipmentNumberException : DomainException
{
    public string ShipmentNumber { get; }
    public override string Code => "invalid_shipment_number";

    public InvalidShipmentNumberException(string shipmentNumber, string message) : base(message)
    {
        ShipmentNumber = shipmentNumber;
    }
}