using AutoMapper;
using AZ.Integrator.Application.Common.ExternalServices.ShipX;
using AZ.Integrator.Application.Common.ExternalServices.ShipX.Models;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Shipments.Commands;

public class CreateInpostShipmentCommandHandler : ICommandHandler<CreateInpostShipmentCommand, ShipmentResponse>
{
    private readonly IShipXService _shipXService;
    private readonly IMapper _mapper;

    public CreateInpostShipmentCommandHandler(IShipXService shipXService, IMapper mapper)
    {
        _shipXService = shipXService;
        _mapper = mapper;
    }
    
    public async ValueTask<ShipmentResponse> Handle(CreateInpostShipmentCommand command, CancellationToken cancellationToken)
    {
        var shipment = _mapper.Map<Shipment>(command);

        var response = await _shipXService.CreateShipment(shipment);
        
        // TODO: Save in DB ?

        return response;
    }
}