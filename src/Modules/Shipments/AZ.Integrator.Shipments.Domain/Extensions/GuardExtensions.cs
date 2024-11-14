using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.Exceptions;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Exceptions;

namespace AZ.Integrator.Shipments.Domain.Extensions;

public static class GuardExtensions
{
    
    public static string InpostTrackingNumber(this IGuardClause guardClause, string input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new InvalidTrackingNumberException(input, "Tracking Number cannot be empty");

        return input;
    }
    
    public static string ShipmentNumber(this IGuardClause guardClause, string input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new InvalidShipmentNumberException(input, "Shipment number cannot be empty");

        return input;
    }
    
    public static long DpdSessionNumber(this IGuardClause guardClause, long input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        if (input == default)
            throw new InvalidSessionNumberException(input, "DPD Session number cannot be empty");

        return input;
    }
    
    public static long DpdPackageNumber(this IGuardClause guardClause, long input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        if (input == default)
            throw new InvalidPackageNumberException(input, "DPD Package number cannot be empty");

        return input;
    }
    
    public static long DpdParcelNumber(this IGuardClause guardClause, long input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        if (input == default)
            throw new InvalidPackageNumberException(input, "DPD Parcel number cannot be empty");

        return input;
    }
    
    public static string DpdWaybill(this IGuardClause guardClause, string input, [CallerArgumentExpression("input")] string parameterName = null)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new InvalidWaybillException(input, "DPD Parcel Waybill cannot be empty");

        return input;
    }
}