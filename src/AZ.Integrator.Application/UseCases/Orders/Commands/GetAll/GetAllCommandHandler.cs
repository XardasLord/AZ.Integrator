using AZ.Integrator.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Orders.Commands.GetAll;

public class GetAllCommandHandler : ICommandHandler<GetAllCommand, IEnumerable<OrderListDto>>
{
    private readonly IAllegroService _allegroService;

    public GetAllCommandHandler(IAllegroService allegroService)
    {
        _allegroService = allegroService;
    }
    
    public async ValueTask<IEnumerable<OrderListDto>> Handle(GetAllCommand command, CancellationToken cancellationToken)
    {
        var orderEvents = await _allegroService.GetOrdersReadyForProcessing();
        
        return orderEvents;
    }
}