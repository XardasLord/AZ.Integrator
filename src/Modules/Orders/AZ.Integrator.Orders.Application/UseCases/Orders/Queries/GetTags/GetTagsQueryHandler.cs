using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Erli;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetTags;

public class GetTagsQueryHandler(
    IAllegroService allegroService,
    IErliService erliService,
    ICurrentUser currentUser) : IRequestHandler<GetTagsQuery, GetTagsResponse>
{
    public async ValueTask<GetTagsResponse> Handle(GetTagsQuery query, CancellationToken cancellationToken)
    {
        if (currentUser.ShopProviderType == ShopProviderType.Allegro)
        {
            var offersResponse = await allegroService.GetOffers(query.Filters);
            var signatures = offersResponse.Offers
                .Where(x => x.External is not null)
                .Select(x => x.External.Id)
                .ToList();

            return new GetTagsResponse(signatures, offersResponse.Count);
        }

        if (currentUser.ShopProviderType == ShopProviderType.Erli)
        {
            var productsResponse = await erliService.GetProducts(query.Filters);
            var tags = productsResponse.Products
                .Where(x => x.Sku is not null)
                .Select(x => x.Sku)
                .ToList();
            
            return new GetTagsResponse(tags, productsResponse.Count);
        }

        return new GetTagsResponse([], 0);
    }
}