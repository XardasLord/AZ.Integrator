using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetTags;

public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, GetTagsResponse>
{
    private readonly IAllegroService _allegroService;

    public GetTagsQueryHandler(IAllegroService allegroService)
    {
        _allegroService = allegroService;
    }
    
    public async ValueTask<GetTagsResponse> Handle(GetTagsQuery query, CancellationToken cancellationToken)
    {
        var offers = await _allegroService.GetOffers(query.Filters);

        var signatures = offers.Offers
            .Where(x => x.External is not null)
            .Select(x => x.External.Id);

        return new GetTagsResponse(signatures, offers.TotalCount);
    }
}