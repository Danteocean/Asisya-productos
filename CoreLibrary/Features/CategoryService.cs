using AutoMapper;
using CoreLibrary.DTOs.Category.Requests;
using CoreLibrary.DTOs.Category.Response;
using CoreLibrary.Interface.Repositories;
using CoreLibrary.Interface.Services;
using Domain.Entities;
using Domain.Querys;
using Domain.Wrappers;

namespace CoreLibrary.Features;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<List<CategoryDtoResponse>>> GetCategories()
    {
        try
        {
            var (data, message) = await _unitOfWork.Queries.QueryAsync<CategoryDtoResponse>(SqlQueries.GetCategory);

            if (data != null)
            {
                return new Response<List<CategoryDtoResponse>>(data.ToList())
                { State = "Ok", Succeeded = true };
            }

            return new Response<List<CategoryDtoResponse>>(null)
            { State = "NoData", Message = "No hay categorías", Succeeded = true };
        }
        catch (Exception ex)
        {
            return new Response<List<CategoryDtoResponse>>(null)
            { State = "Error", Message = ex.Message, Succeeded = false };
        }
    }

    public async Task<Response<bool>> InsertCategory(CategoryAddDtoRequest request)
    {
        try
        {
          

            var category = _mapper.Map<Category>(request);
            category.CreatedAt = DateTime.UtcNow;
            category.IsActive = true;

            await _unitOfWork.Repository<Category>().AddAsync(category);
      

            return new Response<bool>(true)
            { State = "Ok", Message = "Categoría creada correctamente", Succeeded = true };
        }
        catch (Exception ex)
        {
            return new Response<bool>(false)
            { State = "Error", Message = ex.Message, Succeeded = false };
        }
    }

    public async Task<Response<CategoryDtoResponse>> GetCategoryById(int id)
    {
        try
        {
            var (data, message) = await _unitOfWork.Queries.QueryFirstOrDefaultAsync<CategoryDtoResponse>(
                SqlQueries.GetCategoryById,
                new { Id = id });

            if (data != null)
            {
                return new Response<CategoryDtoResponse>(data)
                { State = "Ok", Succeeded = true };
            }

            return new Response<CategoryDtoResponse>(null)
            { State = "NotFound", Message = "La categoría no existe", Succeeded = false };
        }
        catch (Exception ex)
        {
            return new Response<CategoryDtoResponse>(null)
            { State = "Error", Message = ex.Message, Succeeded = false };
        }
    }
}