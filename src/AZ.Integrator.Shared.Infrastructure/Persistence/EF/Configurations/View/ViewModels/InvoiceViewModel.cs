namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels;

public class InvoiceViewModel
{
    public long InvoiceId { get; set; }
    public string InvoiceNumber { get; set; }
    public string AllegroOrderNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}