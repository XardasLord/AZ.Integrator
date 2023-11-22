using AutoMapper;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using MediatR;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetDetails;

public class GetDetailsQueryHandler : IRequestHandler<GetDetailsQuery, OrderDetailsDto>
{
    private readonly IAllegroService _allegroService;
    private readonly IMapper _mapper;

    public GetDetailsQueryHandler(IAllegroService allegroService, IMapper mapper)
    {
        _allegroService = allegroService;
        _mapper = mapper;
    }
    
    public async Task<OrderDetailsDto> Handle(GetDetailsQuery query, CancellationToken cancellationToken)
    {
        var orderDetails = await _allegroService.GetOrderDetails(query.OrderId);

        var orderDetailsDto = _mapper.Map<OrderDetailsDto>(orderDetails);
        
        return orderDetailsDto;
    }
}