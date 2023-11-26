using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.Dpd;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.Dpd.Models;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;
using MediatR;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.Commands.CreateDpdShipment;

public class CreateDpdShipmentCommandHandler : IRequestHandler<CreateDpdShipmentCommand, CreateDpdShipmentResponse>
{
    private readonly IAggregateRepository<DpdShipment> _shipmentRepository;
    private readonly IDpdService _dpdService;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentDateTime _currentDateTime;

    public CreateDpdShipmentCommandHandler(
        IAggregateRepository<DpdShipment> shipmentRepository,
        IDpdService dpdService,
        ICurrentUser currentUser,
        ICurrentDateTime currentDateTime)
    {
        _shipmentRepository = shipmentRepository;
        _dpdService = dpdService;
        _currentUser = currentUser;
        _currentDateTime = currentDateTime;
    }
    
    public async Task<CreateDpdShipmentResponse> Handle(CreateDpdShipmentCommand command, CancellationToken cancellationToken)
    {
        var response = await _dpdService.CreateShipment(command);

        var packages = new List<DpdPackage>();
        foreach (var packageResponse in response.Packages)
        {
            var parcels = packageResponse.Parcels.Select(parcelResponse => DpdParcel.Register(parcelResponse.ParcelId, parcelResponse.Waybill)).ToList();

            packages.Add(DpdPackage.Register(packageResponse.PackageId, parcels));
        }
        
        var dpdShipment = DpdShipment.Create(response.SessionId, packages, command.AllegroOrderId, _currentUser, _currentDateTime);
        await _shipmentRepository.AddAsync(dpdShipment, cancellationToken);

        return response;
    }
}