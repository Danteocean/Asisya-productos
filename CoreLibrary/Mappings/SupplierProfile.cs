using AutoMapper;
using CoreLibrary.DTOs.Supplier.Requests;
using Domain.Entities;

namespace CoreLibrary.Mappings;

public class SupplierProfile : Profile
{
    public SupplierProfile()
    {
        CreateMap<SupplierAddDtoRequest, Supplier>()
    .ForMember(dest => dest.SupplierId, opt => opt.Ignore())
    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}