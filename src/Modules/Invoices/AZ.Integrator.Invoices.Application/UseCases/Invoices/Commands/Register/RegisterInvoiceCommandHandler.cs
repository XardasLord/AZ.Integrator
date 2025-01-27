using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using Mediator;

namespace AZ.Integrator.Invoices.Application.UseCases.Invoices.Commands.Register;

public class RegisterInvoiceCommandHandler : IRequestHandler<RegisterInvoiceCommand>
{
    private readonly IInvoiceService _invoiceService;
    private readonly IAllegroService _allegroService;
    private readonly IAggregateRepository<Invoice> _invoiceRepository;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentDateTime _currentDateTime;

    public RegisterInvoiceCommandHandler(
        IInvoiceService invoiceService,
        IAllegroService allegroService,
        IAggregateRepository<Invoice> invoiceRepository,
        ICurrentUser currentUser,
        ICurrentDateTime currentDateTime)
    {
        _invoiceService = invoiceService;
        _allegroService = allegroService;
        _invoiceRepository = invoiceRepository;
        _currentUser = currentUser;
        _currentDateTime = currentDateTime;
    }
    
    public async ValueTask<Unit> Handle(RegisterInvoiceCommand command, CancellationToken cancellationToken)
    {
        var orderDetails = await _allegroService.GetOrderDetails(Guid.Parse(command.OrderNumber), command.TenantId);
        
        var response = await _invoiceService.GenerateInvoice(orderDetails.Buyer, orderDetails.LineItems, orderDetails.Payment, orderDetails.Delivery);
        
        var invoice = Invoice.Create(response.Id, response.Number, command.OrderNumber, command.TenantId, _currentUser, _currentDateTime);
        await _invoiceRepository.AddAsync(invoice, cancellationToken);

        return Unit.Value;
    }
}