namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels;

public class InpostShipmentViewModel
{
    public string ShipmentNumber { get; set; }
    public string AllegroOrderNumber { get; set; }
    public string TrackingNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}