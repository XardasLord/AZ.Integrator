namespace AZ.Integrator.Invoices.Contracts.Dtos;

public class GetInvoiceResponse
{
    public string InvoiceNumber { get; set; }
    public byte[] File { get; set; }
}