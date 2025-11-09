namespace AZ.Integrator.Shipments.Infrastructure.Persistence.EF.View.ViewModels;

public class DpdShipmentViewModel
{
    public string ShipmentNumber { get; set; }
    public string ExternalOrderNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}