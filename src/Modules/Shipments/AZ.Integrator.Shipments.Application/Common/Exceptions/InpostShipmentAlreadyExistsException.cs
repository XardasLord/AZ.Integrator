namespace AZ.Integrator.Shipments.Application.Common.Exceptions;

public class InpostShipmentAlreadyExistsException : ShipmentsModuleApplicationException
{
    public string ExternalOrderId { get; }
    
    public override string Code => "inpost_shipment_not_found";

    public InpostShipmentAlreadyExistsException(string externalOrderId)
        : base($"Inpost shipment with ExternalOrderId '{externalOrderId}' already exists.")
    {
        ExternalOrderId = externalOrderId;
    }

    public InpostShipmentAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}