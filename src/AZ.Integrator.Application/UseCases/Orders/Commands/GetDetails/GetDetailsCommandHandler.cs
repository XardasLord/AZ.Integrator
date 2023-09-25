using AutoMapper;
using AZ.Integrator.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Orders.Commands.GetDetails;

public class GetDetailsCommandHandler : ICommandHandler<GetDetailsCommand, OrderDetailsDto>
{
    private readonly IAllegroService _allegroService;
    private readonly IMapper _mapper;

    public GetDetailsCommandHandler(IAllegroService allegroService, IMapper mapper)
    {
        _allegroService = allegroService;
        _mapper = mapper;
    }
    
    public async ValueTask<OrderDetailsDto> Handle(GetDetailsCommand command, CancellationToken cancellationToken)
    {
        var orderDetails = await _allegroService.GetOrderDetails(command.OrderId);

        var orderDetailsDto = _mapper.Map<OrderDetailsDto>(orderDetails);
        
        return orderDetailsDto;
    }
}