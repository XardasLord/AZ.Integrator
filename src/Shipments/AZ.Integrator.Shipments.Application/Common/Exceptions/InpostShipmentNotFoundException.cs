namespace AZ.Integrator.Shipments.Application.Common.Exceptions;

public class InpostShipmentNotFoundException : ShipmentsModuleApplicationException
{
    public override string Code => "inpost_shipment_not_found";

    public InpostShipmentNotFoundException()
    {
    }

    public InpostShipmentNotFoundException(string message) : base(message)
    {
    }

    public InpostShipmentNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}