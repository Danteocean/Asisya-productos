using AutoMapper;
using CoreLibrary.DTOs.Category.Requests;
using CoreLibrary.DTOs.Category.Response;
using Domain.Entities;

namespace Infrastructure.Mappings;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryAddDtoRequest, Category>()
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Category, CategoryDtoResponse>();
    }
}