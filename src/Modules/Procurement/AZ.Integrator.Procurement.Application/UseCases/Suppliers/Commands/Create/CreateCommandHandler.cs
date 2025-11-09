using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Procurement.Application.Common.Exceptions;
using AZ.Integrator.Procurement.Contracts.Suppliers;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier.Specifications;
using Mediator;

namespace AZ.Integrator.Procurement.Application.UseCases.Suppliers.Commands.Create;

public class CreateCommandHandler(
    IAggregateRepository<Supplier> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<CreateCommand, SupplierViewModel>
{
    public async ValueTask<SupplierViewModel> Handle(CreateCommand command, CancellationToken cancellationToken)
    {
        var spec = new SupplierByNameSpec(command.SupplierName, currentUser.TenantId);
        var existingSupplier = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (existingSupplier != null)
            throw new SupplierAlreadyExistsException(command.SupplierName);

        var supplier = Supplier.Create(
            command.SupplierName, 
            command.TelephoneNumber,
            command.Mailboxes.Select(x => new Email(x.Email)).ToList(),
            currentUser, currentDateTime);

        await repository.AddAsync(supplier, cancellationToken);

        return new SupplierViewModel
        {
            Id = (int)supplier.Id.Value,
            Name = supplier.Name,
            TenantId = supplier.TenantId,
            TelephoneNumber = supplier.TelephoneNumber,
            CreatedBy = supplier.CreationInformation.CreatedBy,
            CreatedAt = supplier.CreationInformation.CreatedAt.Date,
            ModifiedBy = supplier.ModificationInformation.ModifiedBy,
            ModifiedAt = supplier.ModificationInformation.ModifiedAt.Date,
            Mailboxes = supplier.Mailboxes.Select(sm => new SupplierMailboxViewModel
            {
                Id = sm.Id,
                SupplierId = (int)supplier.Id.Value,
                Email = sm.Email
            }).ToList()
        };
    }
}