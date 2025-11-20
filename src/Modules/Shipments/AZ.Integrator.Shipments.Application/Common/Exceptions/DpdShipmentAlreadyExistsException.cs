namespace AZ.Integrator.Shipments.Application.Common.Exceptions;

public class DpdShipmentAlreadyExistsException : ShipmentsModuleApplicationException
{
    public string ExternalOrderId { get; }
    
    public override string Code => "dpd_shipment_not_found";

    public DpdShipmentAlreadyExistsException(string externalOrderId)
        : base($"DPD shipment with ExternalOrderId '{externalOrderId}' already exists.")
    {
        ExternalOrderId = externalOrderId;
    }

    public DpdShipmentAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}