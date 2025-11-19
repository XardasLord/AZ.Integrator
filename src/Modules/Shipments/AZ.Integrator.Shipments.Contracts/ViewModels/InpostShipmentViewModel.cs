namespace AZ.Integrator.Shipments.Contracts.ViewModels;

public class InpostShipmentViewModel
{
    public Guid TenantId { get; set; }
    public string ShipmentNumber { get; set; }
    public string ExternalOrderNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}