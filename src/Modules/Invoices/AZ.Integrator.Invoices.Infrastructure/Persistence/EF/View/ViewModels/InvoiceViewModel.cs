namespace AZ.Integrator.Invoices.Infrastructure.Persistence.EF.View.ViewModels;

public class InvoiceViewModel
{
    public long InvoiceId { get; set; }
    public string InvoiceNumber { get; set; }
    public string ExternalOrderNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}