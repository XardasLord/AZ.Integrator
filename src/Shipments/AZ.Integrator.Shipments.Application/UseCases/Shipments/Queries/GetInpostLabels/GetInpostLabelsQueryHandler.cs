using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;
using MediatR;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.Queries.GetInpostLabels;

public class GetInpostLabelQueryHandler : IRequestHandler<GetInpostLabelsQuery, GetDocumentResponse>
{
    private readonly IShipXService _shipXService;

    public GetInpostLabelQueryHandler(IShipXService shipXService)
    {
        _shipXService = shipXService;
    }
    
    public async Task<GetDocumentResponse> Handle(GetInpostLabelsQuery query, CancellationToken cancellationToken)
    {
        var response = await _shipXService.GenerateLabel(query.ShipmentNumber.Select(x => new ShipmentNumber(x)));

        return new GetDocumentResponse(true, "ShipmentLabel.pdf", new MemoryStream(response), "application/pdf");
    }
}