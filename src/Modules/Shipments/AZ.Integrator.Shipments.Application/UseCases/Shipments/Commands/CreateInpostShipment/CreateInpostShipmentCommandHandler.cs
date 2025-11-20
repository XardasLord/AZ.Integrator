using AutoMapper;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
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
    IMapper mapper,
    IMonitoringFacade monitoringFacade,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : IRequestHandler<CreateInpostShipmentCommand, ShipmentViewModel>
{
    public async ValueTask<ShipmentViewModel> Handle(CreateInpostShipmentCommand command, CancellationToken cancellationToken)
    {
        await EnsureShipmentForOrderNotExists(command, cancellationToken);

        var shipment = mapper.Map<Shipment>(command);

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
}