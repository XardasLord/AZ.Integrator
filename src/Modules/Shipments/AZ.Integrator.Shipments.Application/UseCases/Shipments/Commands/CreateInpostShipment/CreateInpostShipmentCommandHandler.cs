using AutoMapper;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Monitoring.Contracts;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX.Models;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;
using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.Commands.CreateInpostShipment;

public class CreateInpostShipmentCommandHandler(
    IAggregateRepository<InpostShipment> shipmentRepository,
    IShipXService shipXService,
    IMapper mapper,
    IMonitoringFacade monitoringFacade,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : IRequestHandler<CreateInpostShipmentCommand, ShipmentResponse>
{
    public async ValueTask<ShipmentResponse> Handle(CreateInpostShipmentCommand command, CancellationToken cancellationToken)
    {
        var shipment = mapper.Map<Shipment>(command);

        var response = await shipXService.CreateShipment(shipment);
        
        var inpostShipment = InpostShipment.Create(
            response.Id.ToString(), command.ExternalOrderId, 
            command.TenantId, command.SourceSystemId,
            currentUser, currentDateTime);
        
        await shipmentRepository.AddAsync(inpostShipment, cancellationToken);
        
        foreach (var @event in inpostShipment.Events)
        {
            await monitoringFacade.LogDomainEvent(
                @event, 
                inpostShipment.CreationInformation.TenantId,
                inpostShipment.CreationInformation.CreatedBy,
                currentUser.UserName,
                inpostShipment.CreationInformation.CreatedAt.DateTime,
                MonitoringSourceModuleEnum.Shipments.Name,
                inpostShipment.Number,
                inpostShipment.Number,
                null!,
                cancellationToken);
        }

        return response;
    }
}