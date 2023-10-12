namespace AZ.Integrator.Application.Common.Exceptions;

public class InpostTrackingNumberNotFoundException : ApplicationException
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