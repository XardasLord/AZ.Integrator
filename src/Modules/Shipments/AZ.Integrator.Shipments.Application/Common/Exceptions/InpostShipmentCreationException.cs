namespace AZ.Integrator.Shipments.Application.Common.Exceptions;

public class InpostShipmentCreationException : ShipmentsModuleApplicationException
{
    public override string Code => "inpost_shipment_creation_error";

    public InpostShipmentCreationException()
    {
    }

    public InpostShipmentCreationException(string message) : base(message)
    {
    }

    public InpostShipmentCreationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}