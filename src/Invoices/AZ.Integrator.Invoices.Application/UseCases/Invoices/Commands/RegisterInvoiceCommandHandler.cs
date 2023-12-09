﻿using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using MediatR;

namespace AZ.Integrator.Invoices.Application.UseCases.Invoices.Commands;

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
    
    public async Task<Unit> Handle(RegisterInvoiceCommand command, CancellationToken cancellationToken)
    {
        var orderDetails = await _allegroService.GetOrderDetails(Guid.Parse(command.AllegroOrderNumber));
        
        var invoiceNumber = await _invoiceService.GenerateInvoice(orderDetails.Buyer, orderDetails.LineItems, orderDetails.Payment, orderDetails.Delivery);
        
        var invoice = Invoice.Create(invoiceNumber, command.AllegroOrderNumber, _currentUser, _currentDateTime);
        await _invoiceRepository.AddAsync(invoice, cancellationToken);
        
        return Unit.Value;
    }
}