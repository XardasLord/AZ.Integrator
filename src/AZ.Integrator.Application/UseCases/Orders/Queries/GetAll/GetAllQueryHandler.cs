using AutoMapper;
using AZ.Integrator.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Orders.Queries.GetAll;

public class GetAllQueryHandler : IQueryHandler<GetAllQuery, GetAllQueryResponse>
{
    private readonly IAllegroService _allegroService;
    private readonly IMapper _mapper;

    public GetAllQueryHandler(IAllegroService allegroService, IMapper mapper)
    {
        _allegroService = allegroService;
        _mapper = mapper;
    }
    
    public async ValueTask<GetAllQueryResponse> Handle(GetAllQuery query, CancellationToken cancellationToken)
    {
        var orderEvents = await _allegroService.GetOrderEvents();

        var orderListDto = _mapper.Map<List<OrderListDto>>(orderEvents);
        
        return new GetAllQueryResponse(orderListDto, orderListDto.Count);
    }
}