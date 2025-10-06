using System.Text.Json;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shared.Application;
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
        var ctx = command.PerformContext;
        
        ctx.Step($"Starting assignment of Inpost tracking number for order - {command.ExternalOrderNumber} in database");
        
        var spec = new InpostShipmentByNumberSpec(command.ShippingNumber);
        var shipping = await inpostShippingRepository.SingleOrDefaultAsync(spec, cancellationToken)
            ?? throw new InpostShipmentNotFoundException();

        ctx.Info("Fetching shipments details from external ShipX service...");
        
        var details = await shipXService.GetDetails(command.ShippingNumber);
        
        ctx.Success($"Shipment details retrieved successfully from ShipX service (ID: {details.Id})");

        if (details.TrackingNumber is null)
        {
            var serializedDetails = JsonSerializer.Serialize(details, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            
            ctx.Warning($"There is no tracking number on the Inpost ShipX API for shipment number - {command.ShippingNumber}. " +
                        $"Response details: {serializedDetails}");
            
            throw new InpostTrackingNumberNotFoundException();
        }
        
        var trackingNumbers = details.Parcels
            .Select(x => new TrackingNumber(x.TrackingNumber.ToString()))
            .ToList();
        
        ctx.Success($"Tracking numbers found for shipment in ShipX API - {string.Join(", ", trackingNumbers)}");
            
        shipping.SetTrackingNumber(trackingNumbers.ToList(), command.TenantId);
        
        ctx.Step("Starting saving tracking numbers in database...");

        await inpostShippingRepository.SaveChangesAsync(cancellationToken);
        
        ctx.Success("Tracking numbers saved in database.");
        
        return Unit.Value;
    }
}