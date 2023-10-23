namespace AZ.Integrator.Infrastructure.Persistence.EF.Configurations.View.ViewModels;

public class DpdShipmentViewModel
{
    public string ShipmentNumber { get; set; }
    public string AllegroOrderNumber { get; set; }
    public string TrackingNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}