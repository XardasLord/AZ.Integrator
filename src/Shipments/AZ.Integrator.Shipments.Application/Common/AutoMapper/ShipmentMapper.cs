using AutoMapper;
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
            .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => new Receiver
            {
                Name = src.Receiver.Name == null ? null : src.Receiver.Name.ToUpper(),
                CompanyName = src.Receiver.CompanyName == null ? null : src.Receiver.CompanyName.ToUpper(),
                FirstName = src.Receiver.FirstName == null ? null : src.Receiver.FirstName.ToUpper(),
                LastName = src.Receiver.LastName == null ? null : src.Receiver.LastName.ToUpper(),
                Email = src.Receiver.Email == null ? null : src.Receiver.Email.ToUpper(),
                Phone = src.Receiver.Phone == null ? null : src.Receiver.Phone.ToUpper(),
                Address = new Address
                {
                    Street = src.Receiver.Address.Street == null ? null : src.Receiver.Address.Street.ToUpper(),
                    BuildingNumber = src.Receiver.Address.BuildingNumber == null ? null : src.Receiver.Address.BuildingNumber.ToUpper(),
                    City = src.Receiver.Address.City == null ? null : src.Receiver.Address.City.ToUpper(),
                    PostCode = src.Receiver.Address.PostCode == null ? null : src.Receiver.Address.PostCode.ToUpper(),
                    CountryCode = src.Receiver.Address.CountryCode == null ? null : src.Receiver.Address.CountryCode.ToUpper()
                }
            }))
            .ForMember(dest => dest.Service, opt => opt.MapFrom(src => "inpost_courier_standard"))
            .ForMember(dest => dest.AdditionalServices, opt => opt.MapFrom(src => new List<string>
            {
                "email", "sms"
            }));
    }
}