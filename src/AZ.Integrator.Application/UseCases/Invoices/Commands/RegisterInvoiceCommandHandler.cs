using AZ.Integrator.Application.Common.ExternalServices.SubiektGT;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Invoices.Commands;

public class RegisterInvoiceCommandHandler : ICommandHandler<RegisterInvoiceCommand>
{
    private readonly ISubiektService _subiektService;

    public RegisterInvoiceCommandHandler(ISubiektService subiektService)
    {
        _subiektService = subiektService;
    }
    
    public async ValueTask<Unit> Handle(RegisterInvoiceCommand command, CancellationToken cancellationToken)
    {
        await _subiektService.GenerateInvoice();
        throw new NotImplementedException();
    }
}