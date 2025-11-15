namespace AZ.Integrator.Shipments.Infrastructure.Persistence.EF.View.ViewModels;

public class ShipmentViewModel
{
    public Guid TenantId { get; set; }
    public string ShipmentNumber { get; set; }
    public string ExternalOrderNumber { get; set; }
    public string ShipmentProvider { get; set; }
    public DateTime CreatedAt { get; set; }
}