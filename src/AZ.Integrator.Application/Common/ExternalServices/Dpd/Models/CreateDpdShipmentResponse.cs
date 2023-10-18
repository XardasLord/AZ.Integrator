namespace AZ.Integrator.Application.Common.ExternalServices.Dpd.Models;

public class CreateDpdShipmentResponse
{
    public long SessionId { get; set; }
    public IEnumerable<CreateDpdShipmentPackageResponse> Packages { get; set; }
}

public class CreateDpdShipmentPackageResponse
{
    public long PackageId { get; set; }
    public IEnumerable<CreateDpdShipmentParcelResponse> Parcels { get; set; }
}

public class CreateDpdShipmentParcelResponse
{
    public long ParcelId { get; set; }
    public string Waybill { get; set; }
}