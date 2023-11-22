using AutoMapper;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

namespace AZ.Integrator.Orders.Application.Common.AutoMapper;

public class OrderDetailsDtoMapper : Profile
{
    public OrderDetailsDtoMapper()
    {
        CreateMap<GetOrderDetailsModelResponse, OrderDetailsDto>();
    }
}