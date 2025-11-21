namespace AZ.Integrator.Invoices.Application.Common.Exceptions;

public class InvoiceGenerationException(string message) : InvoicesModuleApplicationException(message)
{
    public override string Code => "invoice_generation_exception";
}