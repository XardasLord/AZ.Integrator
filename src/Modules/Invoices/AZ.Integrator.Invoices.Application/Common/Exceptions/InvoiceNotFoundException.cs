namespace AZ.Integrator.Invoices.Application.Common.Exceptions;

public class InvoiceNotFoundException : InvoicesModuleApplicationException
{
    public override string Code => "invoice_not_found";

    public InvoiceNotFoundException(string message) : base(message)
    {
    }
}