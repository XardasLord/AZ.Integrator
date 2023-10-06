using AZ.Integrator.Application.Common.ExternalServices.ShipX;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Shipments.Queries.GetInpostLabel;

public class GetInpostLabelQueryHandler : IQueryHandler<GetInpostLabelQuery, GetDocumentResponse>
{
    private readonly IShipXService _shipXService;

    public GetInpostLabelQueryHandler(IShipXService shipXService)
    {
        _shipXService = shipXService;
    }
    
    public async ValueTask<GetDocumentResponse> Handle(GetInpostLabelQuery query, CancellationToken cancellationToken)
    {
        var response = await _shipXService.GenerateLabel(query.ShipmentNumber);

        return new GetDocumentResponse(true, "ShipmentLabel.pdf", new MemoryStream(response), "application/pdf");
    }
}