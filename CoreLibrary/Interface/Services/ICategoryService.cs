using CoreLibrary.DTOs.Category.Requests;
using CoreLibrary.DTOs.Category.Response;
using Domain.Wrappers;

namespace CoreLibrary.Interface.Services;

public interface ICategoryService
{
    Task<Response<List<CategoryDtoResponse>>> GetCategories();

    Task<Response<CategoryDtoResponse>> GetCategoryById(int id);

    Task<Response<bool>> InsertCategory(CategoryAddDtoRequest request);
}