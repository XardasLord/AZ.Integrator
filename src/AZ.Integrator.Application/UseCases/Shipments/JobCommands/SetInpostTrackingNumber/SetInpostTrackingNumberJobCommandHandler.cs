using AZ.Integrator.Application.Common.Exceptions;
using AZ.Integrator.Application.Common.ExternalServices.ShipX;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.Aggregates.InpostShipment;
using AZ.Integrator.Domain.Aggregates.InpostShipment.Specifications;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Shipments.JobCommands.SetInpostTrackingNumber;

public class SetInpostTrackingNumberJobCommandHandler : ICommandHandler<SetInpostTrackingNumberJobCommand>
{
    private readonly IAggregateRepository<InpostShipment> _inpostShippingRepository;
    private readonly IShipXService _shipXService;

    public SetInpostTrackingNumberJobCommandHandler(IAggregateRepository<InpostShipment> inpostShippingRepository, IShipXService shipXService)
    {
        _inpostShippingRepository = inpostShippingRepository;
        _shipXService = shipXService;
    }
    
    public async ValueTask<Unit> Handle(SetInpostTrackingNumberJobCommand command, CancellationToken cancellationToken)
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