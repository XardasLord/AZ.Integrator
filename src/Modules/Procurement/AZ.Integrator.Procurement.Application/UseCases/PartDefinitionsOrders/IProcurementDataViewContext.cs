using AZ.Integrator.Procurement.Contracts.PartDefinitionOrders;
using AZ.Integrator.Procurement.Contracts.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders;

public interface IProcurementDataViewContext
{
    DbSet<SupplierViewModel> Suppliers { get; set; }
    DbSet<PartDefinitionsOrderViewModel> Orders { get; set; }
}