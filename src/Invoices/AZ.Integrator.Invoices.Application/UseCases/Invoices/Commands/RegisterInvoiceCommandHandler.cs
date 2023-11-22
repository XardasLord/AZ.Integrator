using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.SubiektGT;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using MediatR;

namespace AZ.Integrator.Invoices.Application.UseCases.Invoices.Commands;

public class RegisterInvoiceCommandHandler : IRequestHandler<RegisterInvoiceCommand>
{
    private readonly ISubiektService _subiektService;
    private readonly IAllegroService _allegroService;
    private readonly IAggregateRepository<Invoice> _invoiceRepository;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentDateTime _currentDateTime;

    public RegisterInvoiceCommandHandler(
        ISubiektService subiektService,
        IAllegroService allegroService,
        IAggregateRepository<Invoice> invoiceRepository,
        ICurrentUser currentUser,
        ICurrentDateTime currentDateTime)
    {
        _subiektService = subiektService;
        _allegroService = allegroService;
        _invoiceRepository = invoiceRepository;
        _currentUser = currentUser;
        _currentDateTime = currentDateTime;
    }
    
    public async Task<Unit> Handle(RegisterInvoiceCommand command, CancellationToken cancellationToken)
    {
        var orderDetails = await _allegroService.GetOrderDetails(Guid.Parse(command.AllegroOrderNumber));
        
        var invoiceNumber = await _subiektService.GenerateInvoice(orderDetails.Id, orderDetails.Buyer, orderDetails.LineItems, orderDetails.Summary, orderDetails.Payment, orderDetails.Delivery);
        
        var invoice = Invoice.Create(invoiceNumber, command.AllegroOrderNumber, _currentUser, _currentDateTime);
        await _invoiceRepository.AddAsync(invoice, cancellationToken);
        
        return Unit.Value;
    }
}