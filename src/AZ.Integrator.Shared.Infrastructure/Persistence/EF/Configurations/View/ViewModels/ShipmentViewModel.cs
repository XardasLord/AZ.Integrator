namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels;

public class ShipmentViewModel
{
    public string ShipmentNumber { get; set; }
    public string AllegroOrderNumber { get; set; }
    public string TrackingNumber { get; set; }
    public string ShipmentProvider { get; set; }
    public DateTime CreatedAt { get; set; }
}