using AutoMapper;
using AZ.Integrator.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Orders.Commands.GetAll;

public class GetAllCommandHandler : ICommandHandler<GetAllCommand, GetAllCommandResponse>
{
    private readonly IAllegroService _allegroService;
    private readonly IMapper _mapper;

    public GetAllCommandHandler(IAllegroService allegroService, IMapper mapper)
    {
        _allegroService = allegroService;
        _mapper = mapper;
    }
    
    public async ValueTask<GetAllCommandResponse> Handle(GetAllCommand command, CancellationToken cancellationToken)
    {
        var orderEvents = await _allegroService.GetOrderEvents();

        orderEvents = orderEvents.Where(x => x.Type == OrderTypes.ReadyForProcessing);

        var orderListDto = _mapper.Map<List<OrderListDto>>(orderEvents);
        
        return new GetAllCommandResponse(orderListDto, orderListDto.Count);
    }
}