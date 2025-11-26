using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Integrations.Contracts;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Monitoring.Contracts;
using AZ.Integrator.Shipments.Application.Common.Exceptions;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX.Models;
using AZ.Integrator.Shipments.Contracts.ViewModels;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Specifications;
using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.Commands.CreateInpostShipment;

public class CreateInpostShipmentCommandHandler(
    IAggregateRepository<InpostShipment> shipmentRepository,
    IShipXService shipXService,
    IMonitoringFacade monitoringFacade,
    IIntegrationsReadFacade integrationsReadFacade,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : IRequestHandler<CreateInpostShipmentCommand, ShipmentViewModel>
{
    public async ValueTask<ShipmentViewModel> Handle(CreateInpostShipmentCommand command, CancellationToken cancellationToken)
    {
        await EnsureShipmentForOrderNotExists(command, cancellationToken);

        var integrationDetails = await integrationsReadFacade.GetActiveInpostIntegrationDetails(currentUser.TenantId, cancellationToken)
                                 ?? throw new InpostShipmentCreationException($"Inpost integration details not found for tenant '{currentUser.TenantId}'.");

        var shipment = MapToShipment(command, integrationDetails);

        var response = await shipXService.CreateShipment(shipment);
        
        var inpostShipment = InpostShipment.Create(
            response.Id.ToString(), command.ExternalOrderId, 
            command.TenantId, command.SourceSystemId,
            currentUser, currentDateTime);

        var events = inpostShipment.Events.ToList();
        
        await shipmentRepository.AddAsync(inpostShipment, cancellationToken);
        
        foreach (var @event in events)
        {
            await monitoringFacade.LogDomainEvent(
                @event, 
                inpostShipment.CreationInformation.TenantId,
                inpostShipment.CreationInformation.SourceSystemId,
                inpostShipment.CreationInformation.CreatedBy,
                currentUser.UserName,
                inpostShipment.CreationInformation.CreatedAt.DateTime,
                MonitoringSourceModuleEnum.Shipments.Name,
                inpostShipment.Number,
                inpostShipment.Number,
                (@event is ITrackableNotification notification ? notification.CorrelationId : null) ?? string.Empty,
                cancellationToken);
        }

        return new ShipmentViewModel
        {
            TenantId = inpostShipment.CreationInformation.TenantId,
            CreatedAt = inpostShipment.CreationInformation.CreatedAt.DateTime,
            ExternalOrderNumber = inpostShipment.ExternalOrderNumber,
            ShipmentNumber = inpostShipment.Number,
            ShipmentProvider = ShipmentProviders.Inpost
        };
    }

    private async ValueTask EnsureShipmentForOrderNotExists(CreateInpostShipmentCommand command, CancellationToken cancellationToken)
    {
        var spec = new InpostShipmentByExternalOrderNumberSpec(command.ExternalOrderId, currentUser.TenantId);
        var exists = await shipmentRepository.AnyAsync(spec, cancellationToken);
        
        if (exists)
            throw new InpostShipmentAlreadyExistsException(command.ExternalOrderId);
    }
    
    private Shipment MapToShipment(CreateInpostShipmentCommand command, InpostIntegrationViewModel integrationDetails)
    {
        return new Shipment
        {
            Sender = new Sender
            {
                Name = integrationDetails.SenderName,
                CompanyName = integrationDetails.SenderCompanyName,
                FirstName = integrationDetails.SenderFirstName,
                LastName = integrationDetails.SenderLastName,
                Email = integrationDetails.SenderEmail,
                Phone = integrationDetails.SenderPhone,
                Address = new Address
                {
                    Street = integrationDetails.SenderAddressStreet,
                    BuildingNumber = integrationDetails.SenderAddressBuildingNumber,
                    City = integrationDetails.SenderAddressCity,
                    PostCode = integrationDetails.SenderAddressPostCode,
                    CountryCode = integrationDetails.SenderAddressCountryCode
                }
            },
            Receiver = new Receiver
            {
                Name = command.Receiver.Name?.ToUpper(),
                CompanyName = command.Receiver.CompanyName?.ToUpper(),
                FirstName = command.Receiver.FirstName?.ToUpper(),
                LastName = command.Receiver.LastName?.ToUpper(),
                Email = command.Receiver.Email?.ToUpper(),
                Phone = command.Receiver.Phone?.ToUpper(),
                Address = new Address
                {
                    Street = command.Receiver.Address.Street?.ToUpper(),
                    BuildingNumber = command.Receiver.Address.BuildingNumber?.ToUpper(),
                    City = command.Receiver.Address.City?.ToUpper(),
                    PostCode = command.Receiver.Address.PostCode?.ToUpper(),
                    CountryCode = command.Receiver.Address.CountryCode?.ToUpper()
                }
            },
            Service = "inpost_courier_standard",
            AdditionalServices = ["sms"]
        };
    }
}