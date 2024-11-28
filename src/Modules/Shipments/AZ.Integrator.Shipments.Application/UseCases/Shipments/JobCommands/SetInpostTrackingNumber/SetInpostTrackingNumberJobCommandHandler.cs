using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shipments.Application.Common.Exceptions;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Specifications;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;
using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.SetInpostTrackingNumber;

public class SetInpostTrackingNumberJobCommandHandler(
    IAggregateRepository<InpostShipment> inpostShippingRepository,
    IShipXService shipXService) 
    : IRequestHandler<SetInpostTrackingNumberJobCommand>
{
    public async ValueTask<Unit> Handle(SetInpostTrackingNumberJobCommand command, CancellationToken cancellationToken)
    {
        var spec = new InpostShipmentByNumberSpec(command.ShippingNumber);
        var shipping = await inpostShippingRepository.SingleOrDefaultAsync(spec, cancellationToken)
            ?? throw new InpostShipmentNotFoundException();

        var details = await shipXService.GetDetails(command.ShippingNumber);

        if (details.TrackingNumber is null)
            throw new InpostTrackingNumberNotFoundException();

        var trackingNumbers = details.Parcels.Select(x => new TrackingNumber(x.TrackingNumber.ToString()));
            
        shipping.SetTrackingNumber(trackingNumbers.ToList(), command.TenantId);

        await inpostShippingRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}