using AZ.Integrator.Application.Common.ExternalServices.Dpd;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Shipments.Queries.GetDpdLabel;

public class GetDpdLabelQueryHandler : IQueryHandler<GetDpdLabelQuery, GetDocumentResponse>
{
    private readonly IDpdService _dpdService;

    public GetDpdLabelQueryHandler(IDpdService dpdService)
    {
        _dpdService = dpdService;
    }
    
    public async ValueTask<GetDocumentResponse> Handle(GetDpdLabelQuery query, CancellationToken cancellationToken)
    {
        var response = await _dpdService.GenerateLabel(query.SessionNumber);

        return new GetDocumentResponse(true, "ShipmentLabel.pdf", new MemoryStream(response), "application/pdf");
    }
}