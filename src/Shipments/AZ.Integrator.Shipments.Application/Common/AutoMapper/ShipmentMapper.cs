﻿using AutoMapper;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX.Models;
using AZ.Integrator.Shipments.Application.UseCases.Shipments.Commands.CreateInpostShipment;

namespace AZ.Integrator.Shipments.Application.Common.AutoMapper;

public class ShipmentMapper : Profile
{
    public ShipmentMapper()
    {
        CreateMap<CreateInpostShipmentCommand, Shipment>()
            .ForMember(dest => dest.Service, opt => opt.MapFrom(src => "inpost_courier_standard"))
            .ForMember(dest => dest.AdditionalServices, opt => opt.MapFrom(src => new List<string>
            {
                "email", "sms"
            }));
    }
}