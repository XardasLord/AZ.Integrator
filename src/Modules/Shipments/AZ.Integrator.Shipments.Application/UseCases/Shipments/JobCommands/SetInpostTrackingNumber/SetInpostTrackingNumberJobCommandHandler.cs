using System.Text.Json;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Shipments.Application.Common.Exceptions;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Specifications;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;
using Hangfire.Console;
using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.SetInpostTrackingNumber;

public class SetInpostTrackingNumberJobCommandHandler(
    IAggregateRepository<InpostShipment> inpostShippingRepository,
    IShipXService shipXService) 
    : IRequestHandler<SetInpostTrackingNumberJobCommand>
{
    public async ValueTask<Unit> Handle(SetInpostTrackingNumberJobCommand command, CancellationToken cancellationToken)
    {
        command.PerformContext.WriteLine($"Starting assigning Inpost tracking number for order in database - {command.ExternalOrderNumber}");
        
        var spec = new InpostShipmentByNumberSpec(command.ShippingNumber);
        var shipping = await inpostShippingRepository.SingleOrDefaultAsync(spec, cancellationToken)
            ?? throw new InpostShipmentNotFoundException();

        var details = await shipXService.GetDetails(command.ShippingNumber);

        if (details.TrackingNumber is null)
        {
            var serializedDetails = JsonSerializer.Serialize(details, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            
            command.PerformContext.SetTextColor(ConsoleTextColor.DarkRed);
            command.PerformContext.WriteLine(
                $"There is no tracking number on the Inpost ShipX API for shipment number - {command.ShippingNumber}. " +
                $"Response details: {serializedDetails}"
            );
            
            throw new InpostTrackingNumberNotFoundException();
        }
        
        var trackingNumbers = details.Parcels
            .Select(x => new TrackingNumber(x.TrackingNumber.ToString()))
            .ToList();
        
        command.PerformContext.ResetTextColor();
        command.PerformContext.WriteLine($"Tracking numbers found for shipment in ShipX API - {string.Join(", ", trackingNumbers)}");
            
        shipping.SetTrackingNumber(trackingNumbers.ToList(), command.TenantId);

        await inpostShippingRepository.SaveChangesAsync(cancellationToken);
        
        command.PerformContext.SetTextColor(ConsoleTextColor.DarkGreen);
        command.PerformContext.WriteLine("Tracking numbers saved in database.");
        
        return Unit.Value;
    }
}