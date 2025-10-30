using AZ.Integrator.Procurement.Contracts.Suppliers;
using Mediator;

namespace AZ.Integrator.Procurement.Application.UseCases.Suppliers.Commands.Create;

public record CreateCommand(string SupplierName, string TelephoneNumber, IEnumerable<SupplierMailboxRequest> Mailboxes) : ICommand<SupplierViewModel>;