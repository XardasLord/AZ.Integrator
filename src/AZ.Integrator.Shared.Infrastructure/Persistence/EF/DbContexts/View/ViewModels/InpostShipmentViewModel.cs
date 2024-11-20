namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;

public class InpostShipmentViewModel
{
    public string ShipmentNumber { get; set; }
    public string ExternalOrderNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}