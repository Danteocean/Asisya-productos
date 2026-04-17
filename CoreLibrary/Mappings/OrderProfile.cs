using AutoMapper;
using CoreLibrary.DTOs.Order.Requests;
using Domain.Entities;

namespace CoreLibrary.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile() 
    {
        CreateMap<OrderAddDtoRequest, Order>()
    .ForMember(dest => dest.OrderDetails, opt => opt.Ignore());
    }
}
