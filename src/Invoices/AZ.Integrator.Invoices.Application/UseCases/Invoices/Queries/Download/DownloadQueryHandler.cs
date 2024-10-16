using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using Mediator;

namespace AZ.Integrator.Invoices.Application.UseCases.Invoices.Queries.Download;

public class DownloadQueryHandler : IRequestHandler<DownloadQuery, GetDocumentResponse>
{
    private readonly IInvoiceService _invoiceService;

    public DownloadQueryHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }
    
    public async ValueTask<GetDocumentResponse> Handle(DownloadQuery query, CancellationToken cancellationToken)
    {
        var response = await _invoiceService.Download(query.InvoiceId);

        return new GetDocumentResponse(true, "Invoice.pdf", new MemoryStream(response), "application/pdf");
    }
}