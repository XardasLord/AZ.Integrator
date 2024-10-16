using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetAll;

public class GetAllQueryHandler : IRequestHandler<GetAllQuery, GetAllQueryResponse>
{
    private readonly IAllegroService _allegroService;

    public GetAllQueryHandler(IAllegroService allegroService)
    {
        _allegroService = allegroService;
    }
    
    public async ValueTask<GetAllQueryResponse> Handle(GetAllQuery query, CancellationToken cancellationToken)
    {
        var ordersResponse = await _allegroService.GetOrders(query.Filters);

        return new GetAllQueryResponse(ordersResponse.CheckoutForms, ordersResponse.Count, ordersResponse.TotalCount);
    }
}