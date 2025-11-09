namespace AZ.Integrator.Procurement.Application.Common.Exceptions;

public class SupplierAlreadyExistsException(string supplierName) 
    : ProcurementModuleApplicationException($"Supplier with name '{supplierName}' already exists.")
{
    public override string Code => "supplier_not_found";
    
    public string SupplierName { get; } = supplierName;
}