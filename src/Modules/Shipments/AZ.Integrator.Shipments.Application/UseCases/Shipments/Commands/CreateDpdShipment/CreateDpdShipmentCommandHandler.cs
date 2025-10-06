using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.Dpd;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.Dpd.Models;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;
using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.Commands.CreateDpdShipment;

public class CreateDpdShipmentCommandHandler(
    IAggregateRepository<DpdShipment> shipmentRepository,
    IDpdService dpdService,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : IRequestHandler<CreateDpdShipmentCommand, CreateDpdShipmentResponse>
{
    public async ValueTask<CreateDpdShipmentResponse> Handle(CreateDpdShipmentCommand command, CancellationToken cancellationToken)
    {
        var response = await dpdService.CreateShipment(command);

        var packages = new List<DpdPackage>();
        
        foreach (var packageResponse in response.Packages)
        {
            var parcels = packageResponse.Parcels.Select(parcelResponse => DpdParcel.Register(parcelResponse.ParcelId, parcelResponse.Waybill)).ToList();

            packages.Add(DpdPackage.Register(packageResponse.PackageId, parcels));
        }
        
        var dpdShipment = DpdShipment.Create(
            response.SessionId, packages, command.AllegroOrderId, 
            command.TenantId, command.SourceSystemId,
            currentUser, currentDateTime);
        
        await shipmentRepository.AddAsync(dpdShipment, cancellationToken);

        return response;
    }
}