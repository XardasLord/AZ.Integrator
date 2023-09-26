using AutoMapper;
using AZ.Integrator.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Orders.Queries.GetDetails;

public class GetDetailsQueryHandler : IQueryHandler<GetDetailsQuery, OrderDetailsDto>
{
    private readonly IAllegroService _allegroService;
    private readonly IMapper _mapper;

    public GetDetailsQueryHandler(IAllegroService allegroService, IMapper mapper)
    {
        _allegroService = allegroService;
        _mapper = mapper;
    }
    
    public async ValueTask<OrderDetailsDto> Handle(GetDetailsQuery command, CancellationToken cancellationToken)
    {
        var orderDetails = await _allegroService.GetOrderDetails(command.OrderId);

        var orderDetailsDto = _mapper.Map<OrderDetailsDto>(orderDetails);
        
        return orderDetailsDto;
    }
}