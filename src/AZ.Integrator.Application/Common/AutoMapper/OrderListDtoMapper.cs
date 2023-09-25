using AutoMapper;
using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;

namespace AZ.Integrator.Application.Common.AutoMapper;

public class OrderListDtoMapper : Profile
{
    public OrderListDtoMapper()
    {
        CreateMap<Buyer, BuyerDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.Guest, opt => opt.MapFrom(src => src.Guest));

        CreateMap<OriginalPrice, PriceDto>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency));
        
        CreateMap<Price, PriceDto>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency));
        
        CreateMap<LineItem, LineItemDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Offer.Id))
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Offer.Name));

        CreateMap<OrderEvent, OrderListDto>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.OccurredAt))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Order.CheckoutForm.Id))
            .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => src.Order.Buyer))
            .ForMember(dest => dest.LineItems, opt => opt.MapFrom(src => src.Order.LineItems));
    }
}