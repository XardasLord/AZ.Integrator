namespace AZ.Integrator.Procurement.Application.Common.Exceptions;

public class SupplierNotFoundException(string message) : ProcurementModuleApplicationException(message)
{
    public override string Code => "supplier_not_found";
}