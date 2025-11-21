using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using Mediator;

namespace AZ.Integrator.Invoices.Application.UseCases.Invoices.Queries.Download;

public class DownloadQueryHandler(IInvoiceService invoiceService, ICurrentUser currentUser)
    : IRequestHandler<DownloadQuery, GetDocumentResponse>
{
    public async ValueTask<GetDocumentResponse> Handle(DownloadQuery query, CancellationToken cancellationToken)
    {
        var response = await invoiceService.Download(query.InvoiceId, currentUser.TenantId);

        return new GetDocumentResponse(true, "Invoice.pdf", new MemoryStream(response), "application/pdf");
    }
}