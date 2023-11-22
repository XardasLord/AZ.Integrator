using AZ.Integrator.Domain.SharedKernel.Exceptions;

namespace AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.Exceptions;

public class InvalidWaybillException : DomainException
{
    public string Waybill { get; }
    public override string Code => "invalid_waybill";

    public InvalidWaybillException(string waybill, string message) : base(message)
    {
        Waybill = waybill;
    }
}