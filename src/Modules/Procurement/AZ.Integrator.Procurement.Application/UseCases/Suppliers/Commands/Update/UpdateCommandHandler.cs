using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Procurement.Application.Common.Exceptions;
using AZ.Integrator.Procurement.Contracts.Suppliers;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier.Specifications;
using Mediator;

namespace AZ.Integrator.Procurement.Application.UseCases.Suppliers.Commands.Update;

public class UpdateCommandHandler(
    IAggregateRepository<Supplier> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<UpdateCommand, SupplierViewModel>
{
    public async ValueTask<SupplierViewModel> Handle(UpdateCommand command, CancellationToken cancellationToken)
    {
        var spec = new SupplierByIdSpec(command.SupplierId, currentUser.TenantId);
        var supplier = await repository.FirstOrDefaultAsync(spec, cancellationToken)
            ?? throw new SupplierNotFoundException(command.SupplierName);
        
        supplier.Update(
            command.SupplierName, 
            command.TelephoneNumber,
            command.Mailboxes.Select(x => new Email(x.Email)).ToList(),
            currentUser, currentDateTime);

        await repository.SaveChangesAsync(cancellationToken);

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