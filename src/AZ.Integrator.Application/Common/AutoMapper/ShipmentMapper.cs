using AutoMapper;
using AZ.Integrator.Application.Common.ExternalServices.ShipX.Models;
using AZ.Integrator.Application.UseCases.Shipments.Commands;

namespace AZ.Integrator.Application.Common.AutoMapper;

public class ShipmentMapper : Profile
{
    public ShipmentMapper()
    {
        CreateMap<CreateInpostShipmentCommand, Shipment>();
    }
}