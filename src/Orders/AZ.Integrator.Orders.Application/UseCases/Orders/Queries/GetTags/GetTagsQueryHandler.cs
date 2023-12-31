﻿using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using MediatR;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetTags;

public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, IEnumerable<string>>
{
    private readonly IAllegroService _allegroService;

    public GetTagsQueryHandler(IAllegroService allegroService)
    {
        _allegroService = allegroService;
    }
    
    public async Task<IEnumerable<string>> Handle(GetTagsQuery query, CancellationToken cancellationToken)
    {
        var registeredTags = await _allegroService.GetRegisteredTags();

        return registeredTags.Tags.Select(x => x.Name);
    }
}