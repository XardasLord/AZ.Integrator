using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shipments.Application.Common.Exceptions;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Specifications;
using MediatR;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.SetInpostTrackingNumber;

public class SetInpostTrackingNumberJobCommandHandler : IRequestHandler<SetInpostTrackingNumberJobCommand>
{
    private readonly IAggregateRepository<InpostShipment> _inpostShippingRepository;
    private readonly IShipXService _shipXService;

    public SetInpostTrackingNumberJobCommandHandler(IAggregateRepository<InpostShipment> inpostShippingRepository, IShipXService shipXService)
    {
        _inpostShippingRepository = inpostShippingRepository;
        _shipXService = shipXService;
    }
    
    public async Task<Unit> Handle(SetInpostTrackingNumberJobCommand command, CancellationToken cancellationToken)
    {
        var spec = new InpostShipmentByNumberSpec(command.ShippingNumber);
        var shipping = await _inpostShippingRepository.SingleOrDefaultAsync(spec, cancellationToken)
            ?? throw new InpostShipmentNotFoundException();

        var details = await _shipXService.GetDetails(command.ShippingNumber);

        if (details.TrackingNumber is null)
            throw new InpostTrackingNumberNotFoundException();

        shipping.SetTrackingNumber(details.TrackingNumber.ToString());

        await _inpostShippingRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}