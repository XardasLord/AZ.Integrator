using AutoMapper;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using MediatR;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetOrderProductTags;

public class GetOrderProductTagsQueryHandler : IRequestHandler<GetOrderProductTagsQuery, IEnumerable<string>>
{
    private readonly IAllegroService _allegroService;
    private readonly IMapper _mapper;

    public GetOrderProductTagsQueryHandler(IAllegroService allegroService, IMapper mapper)
    {
        _allegroService = allegroService;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<string>> Handle(GetOrderProductTagsQuery query, CancellationToken cancellationToken)
    {
        var orderDetails = await _allegroService.GetOrderDetails(query.OrderId);

        var result = new List<string>();
        
        foreach (var lineItem in orderDetails.LineItems)
        {
            var offerTags = await _allegroService.GetOfferTags(lineItem.Offer.Id);

            if (offerTags.Tags.Any())
            {
                result.AddRange(offerTags.Tags.Select(x => x.Name));
            }
        }

        return result;
    }
}