using AZ.Integrator.Procurement.Contracts.Suppliers;
using Mediator;

namespace AZ.Integrator.Procurement.Application.UseCases.Suppliers.Commands.Update;

public record UpdateCommand(int SupplierId, string SupplierName, string TelephoneNumber, IEnumerable<SupplierMailboxRequest> Mailboxes) : ICommand<SupplierViewModel>;