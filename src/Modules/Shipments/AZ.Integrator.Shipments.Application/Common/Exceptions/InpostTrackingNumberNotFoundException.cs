namespace AZ.Integrator.Shipments.Application.Common.Exceptions;

public class InpostTrackingNumberNotFoundException : ShipmentsModuleApplicationException
{
    public override string Code => "inpost_shipment_tracking_number_not_generated";

    public InpostTrackingNumberNotFoundException()
    {
    }

    public InpostTrackingNumberNotFoundException(string message) : base(message)
    {
    }

    public InpostTrackingNumberNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}