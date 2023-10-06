namespace AZ.Integrator.Application.UseCases.Shipments.Queries;

public record GetDocumentResponse(bool Exists, string FileName = null, Stream ContentStream = null, string ContentType = null)
{
    public static readonly GetDocumentResponse Empty = new GetDocumentResponse(false);
}