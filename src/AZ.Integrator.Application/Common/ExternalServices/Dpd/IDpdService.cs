using AZ.Integrator.Application.Common.ExternalServices.Dpd.Models;
using AZ.Integrator.Application.UseCases.Shipments.Commands.CreateDpdShipment;

namespace AZ.Integrator.Application.Common.ExternalServices.Dpd;

public interface IDpdService
{
    Task<CreateDpdShipmentResponse> CreateShipment(CreateDpdShipmentCommand shipment);
}