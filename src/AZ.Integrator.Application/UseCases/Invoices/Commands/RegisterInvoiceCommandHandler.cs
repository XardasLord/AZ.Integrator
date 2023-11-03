using AZ.Integrator.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Application.Common.ExternalServices.SubiektGT;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Invoices.Commands;

public class RegisterInvoiceCommandHandler : ICommandHandler<RegisterInvoiceCommand>
{
    private readonly ISubiektService _subiektService;
    private readonly IAllegroService _allegroService;

    public RegisterInvoiceCommandHandler(ISubiektService subiektService, IAllegroService allegroService)
    {
        _subiektService = subiektService;
        _allegroService = allegroService;
    }
    
    public async ValueTask<Unit> Handle(RegisterInvoiceCommand command, CancellationToken cancellationToken)
    {
        var orderDetails = await _allegroService.GetOrderDetails(Guid.Parse(command.AllegroOrderNumber));
        
        var invoiceNumber = await _subiektService.GenerateInvoice(orderDetails.Id, orderDetails.Buyer, orderDetails.LineItems, orderDetails.Summary, orderDetails.Payment);
        
        // TODO: Add invoiceNumber to domain model
        
        return Unit.Value;
    }
}