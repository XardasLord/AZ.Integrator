using Mediator;

namespace AZ.Integrator.Invoices.Application.UseCases.Invoices.Queries.Download;

public record DownloadQuery(long InvoiceId): IRequest<GetDocumentResponse>;