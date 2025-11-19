namespace AZ.Integrator.Shipments.Contracts.ViewModels;

public class ShipmentViewModel
{
    public Guid TenantId { get; set; }
    public string ShipmentNumber { get; set; }
    public string ExternalOrderNumber { get; set; }
    public string ShipmentProvider { get; set; }
    public DateTime CreatedAt { get; set; }
}

public static class ShipmentProviders 
{
    public const string Inpost = "INPOST";
    public const string Dpd = "DPD";
}