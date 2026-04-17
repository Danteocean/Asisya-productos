using AutoMapper;
using CoreLibrary.DTOs.Product.Requests;
using Domain.Entities;

namespace CoreLibrary.Mappings;

public class ProductsProfile : Profile
{
    public ProductsProfile()
    {
        CreateMap<ProductAddDtoRequest, Product>()
    .ForMember(dest => dest.ProductId, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        CreateMap<ProductUpdateDtoRequest, Product>();

    }
}