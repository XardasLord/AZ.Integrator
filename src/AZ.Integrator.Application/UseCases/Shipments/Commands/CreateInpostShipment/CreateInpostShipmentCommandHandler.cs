using AutoMapper;
using AZ.Integrator.Application.Common.ExternalServices.ShipX;
using AZ.Integrator.Application.Common.ExternalServices.ShipX.Models;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.Aggregates.InpostShipment;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Shipments.Commands.CreateInpostShipment;

public class CreateInpostShipmentCommandHandler : ICommandHandler<CreateInpostShipmentCommand, ShipmentResponse>
{
    private readonly IAggregateRepository<InpostShipment> _shipmentRepository;
    private readonly IShipXService _shipXService;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentDateTime _currentDateTime;

    public CreateInpostShipmentCommandHandler(
        IAggregateRepository<InpostShipment> shipmentRepository,
        IShipXService shipXService,
        IMapper mapper,
        ICurrentUser currentUser,
        ICurrentDateTime currentDateTime)
    {
        _shipmentRepository = shipmentRepository;
        _shipXService = shipXService;
        _mapper = mapper;
        _currentUser = currentUser;
        _currentDateTime = currentDateTime;
    }
    
    public async ValueTask<ShipmentResponse> Handle(CreateInpostShipmentCommand command, CancellationToken cancellationToken)
    {
        var shipment = _mapper.Map<Shipment>(command);

        var response = await _shipXService.CreateShipment(shipment);
        
        var inpostShipment = InpostShipment.Create(response.Id.ToString(), response.Reference, _currentUser, _currentDateTime);
        await _shipmentRepository.AddAsync(inpostShipment, cancellationToken);

        return response;
    }
}