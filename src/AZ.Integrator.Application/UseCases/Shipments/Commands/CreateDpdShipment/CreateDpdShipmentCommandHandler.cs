using AZ.Integrator.Application.Common.ExternalServices.Dpd;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.Aggregates.InpostShipment;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Shipments.Commands.CreateDpdShipment;

public class CreateDpdShipmentCommandHandler : ICommandHandler<CreateDpdShipmentCommand, object>
{
    private readonly IAggregateRepository<InpostShipment> _shipmentRepository;
    private readonly IDpdService _dpdService;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentDateTime _currentDateTime;

    public CreateDpdShipmentCommandHandler(
        IAggregateRepository<InpostShipment> shipmentRepository,
        IDpdService dpdService,
        ICurrentUser currentUser,
        ICurrentDateTime currentDateTime)
    {
        _shipmentRepository = shipmentRepository;
        _dpdService = dpdService;
        _currentUser = currentUser;
        _currentDateTime = currentDateTime;
    }
    
    public async ValueTask<object> Handle(CreateDpdShipmentCommand command, CancellationToken cancellationToken)
    {
        var response = await _dpdService.CreateShipment(command);
        
        // var inpostShipment = InpostShipment.Create(response.Id.ToString(), response.Reference, _currentUser, _currentDateTime);
        // await _shipmentRepository.AddAsync(inpostShipment, cancellationToken);

        return response;
    }
}