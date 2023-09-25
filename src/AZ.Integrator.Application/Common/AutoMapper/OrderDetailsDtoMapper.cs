using AutoMapper;
using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;

namespace AZ.Integrator.Application.Common.AutoMapper;

public class OrderDetailsDtoMapper : Profile
{
    public OrderDetailsDtoMapper()
    {
        CreateMap<GetOrderDetailsModel, OrderDetailsDto>();
    }
}