using System.IO.Compression;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX;
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
        // var response = await _shipXService.GenerateLabel(query.ShipmentNumber.Select(x => new ShipmentNumber(x))); // Old way, where all labels are generated in one file
        // return new GetDocumentResponse(true, "ShipmentLabel.pdf", new MemoryStream(response), "application/pdf");

        var response = new List<byte[]>();
        
        foreach (var shipmentNumber in query.ShipmentNumber)
        {
            response.Add(await _shipXService.GenerateLabel(shipmentNumber));
        }
        
        using (var memoryStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                for (int i = 0; i < response.Count; i++)
                {
                    var zipEntry = archive.CreateEntry($"ShipmentLabel{i + 1}.pdf");

                    await using var entryStream = zipEntry.Open();
                    using var fileStream = new MemoryStream(response[i]);
                    
                    await fileStream.CopyToAsync(entryStream, cancellationToken);
                }
            }

            memoryStream.Position = 0;
            
            return new GetDocumentResponse(true, "ShipmentLabels.zip", new MemoryStream(memoryStream.ToArray()), "application/zip");
        }
    }
}