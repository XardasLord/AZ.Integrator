using AZ.Integrator.Shipments.Application.Common.ExternalServices.Dpd.Models;
using AZ.Integrator.Shipments.Application.UseCases.Shipments.Commands.CreateDpdShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Application.Common.ExternalServices.Dpd;

public interface IDpdService
{
    Task<CreateDpdShipmentResponse> CreateShipment(CreateDpdShipmentCommand shipment);
    Task<byte[]> GenerateLabel(SessionNumber sessionNumber);
}