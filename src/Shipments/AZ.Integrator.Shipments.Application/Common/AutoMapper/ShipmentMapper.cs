﻿using AutoMapper;
using AZ.Integrator.Shipments.Application.Common.Configurations;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX.Models;
using AZ.Integrator.Shipments.Application.UseCases.Shipments.Commands.CreateInpostShipment;
using Microsoft.Extensions.Options;

namespace AZ.Integrator.Shipments.Application.Common.AutoMapper;

public class ShipmentMapper : Profile
{
    public ShipmentMapper(IOptions<InpostSenderOptions> options)
    {
        var senderData = options.Value.SenderData;
        var senderAddressData = options.Value.SenderData.Address;
        
        CreateMap<CreateInpostShipmentCommand, Shipment>()
            .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => new Sender
            {
                Name = senderData.Name,
                CompanyName = senderData.CompanyName,
                FirstName = senderData.FirstName,
                LastName = senderData.LastName,
                Email = senderData.Email,
                Phone = senderData.Phone,
                Address = new Address
                {
                    Street = senderAddressData.Street,
                    BuildingNumber = senderAddressData.BuildingNumber,
                    City = senderAddressData.City,
                    PostCode = senderAddressData.PostCode,
                    CountryCode = senderAddressData.CountryCode
                }
            }))
            .ForMember(dest => dest.Service, opt => opt.MapFrom(src => "inpost_courier_standard"))
            .ForMember(dest => dest.AdditionalServices, opt => opt.MapFrom(src => new List<string>
            {
                "email", "sms"
            }));
    }
}