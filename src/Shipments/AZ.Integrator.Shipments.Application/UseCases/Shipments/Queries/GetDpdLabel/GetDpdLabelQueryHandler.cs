using AZ.Integrator.Shipments.Application.Common.ExternalServices.Dpd;
using MediatR;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.Queries.GetDpdLabel;

public class GetDpdLabelQueryHandler : IRequestHandler<GetDpdLabelQuery, GetDocumentResponse>
{
    private readonly IDpdService _dpdService;

    public GetDpdLabelQueryHandler(IDpdService dpdService)
    {
        _dpdService = dpdService;
    }
    
    public async Task<GetDocumentResponse> Handle(GetDpdLabelQuery query, CancellationToken cancellationToken)
    {
        var response = await _dpdService.GenerateLabel(query.SessionNumber);

        return new GetDocumentResponse(true, "ShipmentLabel.pdf", new MemoryStream(response), "application/pdf");
    }
}