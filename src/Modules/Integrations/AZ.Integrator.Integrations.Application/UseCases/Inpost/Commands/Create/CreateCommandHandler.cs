using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Application.Common.Exceptions;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Integrations.Domain.Aggregates.Inpost;
using AZ.Integrator.Integrations.Domain.Aggregates.Inpost.Specifications;
using AZ.Integrator.Integrations.Domain.Aggregates.Inpost.ValueObjects;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Inpost.Commands.Create;

public class CreateCommandHandler(
    IAggregateRepository<InpostIntegration> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<CreateCommand, InpostIntegrationViewModel>
{
    public async ValueTask<InpostIntegrationViewModel> Handle(CreateCommand command, CancellationToken cancellationToken)
    {
        var spec = new InpostIntegrationByOrganizationIdSpec(currentUser.TenantId, command.Request.OrganizationId);
        var exists = await repository.AnyAsync(spec, cancellationToken);

        if (exists)
            throw new IntegrationAlreadyExistsException(currentUser.TenantId, command.Request.OrganizationId.ToString());
        
        var senderData = new SenderData(
            command.Request.SenderName,
            command.Request.SenderCompanyName,
            command.Request.SenderFirstName,
            command.Request.SenderLastName,
            command.Request.SenderEmail,
            command.Request.SenderPhone,
            new SenderDataAddress(
                command.Request.SenderAddressStreet,
                command.Request.SenderAddressBuildingNumber,
                command.Request.SenderAddressCity,
                command.Request.SenderAddressPostCode,
                command.Request.SenderAddressCountryCode)
            );
        
        var integration = InpostIntegration.Create(
            command.Request.OrganizationId,
            command.Request.AccessToken,
            command.Request.DisplayName,
            senderData,
            currentUser, currentDateTime);

        await repository.AddAsync(integration, cancellationToken);

        return new InpostIntegrationViewModel
        {
            TenantId = integration.TenantId,
            OrganizationId = integration.OrganizationId,
            AccessToken = integration.AccessToken,
            DisplayName = integration.DisplayName,
            IsEnabled = integration.IsEnabled,
            CreatedAt = integration.CreatedAt,
            UpdatedAt = integration.UpdatedAt,
            SenderName = integration.SenderData.Name,
            SenderCompanyName = integration.SenderData.CompanyName,
            SenderFirstName = integration.SenderData.FirstName,
            SenderLastName = integration.SenderData.LastName,
            SenderEmail = integration.SenderData.Email,
            SenderPhone = integration.SenderData.Phone,
            SenderAddressStreet = integration.SenderData.Address.Street,
            SenderAddressBuildingNumber = integration.SenderData.Address.BuildingNumber,
            SenderAddressCity = integration.SenderData.Address.City,
            SenderAddressPostCode = integration.SenderData.Address.PostCode,
            SenderAddressCountryCode = integration.SenderData.Address.CountryCode
        };
    }
}