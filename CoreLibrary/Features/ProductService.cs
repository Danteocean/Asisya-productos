using AutoMapper;
using Bogus;
using CoreLibrary.DTOs.Asisya.Requests;
using CoreLibrary.DTOs.Asisya.Response;
using CoreLibrary.DTOs.Product.Requests;
using CoreLibrary.Interface.Repositories;
using CoreLibrary.Interface.Services;
using Domain.Entities;
using Domain.Querys;
using Domain.Wrappers;

namespace CoreLibrary.Features;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<List<ProductDtoResponse>>> GetProducts(ProductDtoRequest request)
    {
        try
        {
            request.PageNumber = request.PageNumber == 0 ? 1 : request.PageNumber;
            request.PageSize = request.PageSize == 0 ? 10 : request.PageSize;

            var (data, message) = await _unitOfWork.Queries.QueryAsync<ProductDtoResponse>(
                SqlQueries.GetProductsPaged,
                new
                {
                    Search = string.IsNullOrWhiteSpace(request.Search) ? null : request.Search,
                    CategoryId = request.CategoryId,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                });

            if (data != null)
            {
                var result = _mapper.Map<List<ProductDtoResponse>>(data);
                return new Response<List<ProductDtoResponse>>(result)
                { State = "Ok", Message = message, Succeeded = true };
            }

            return new Response<List<ProductDtoResponse>>(null)
            { State = "NoData", Message = "No se encontraron productos", Succeeded = true };
        }
        catch (Exception ex)
        {
            return new Response<List<ProductDtoResponse>>(null)
            { State = "Error", Message = ex.Message, Succeeded = false };
        }
    }

    public async Task<Response<ProductDtoResponse>> GetProductById(ProductIdDtoRequest request)
    {
        try
        {
            var (data, message) = await _unitOfWork.Queries.QueryFirstOrDefaultAsync<ProductDtoResponse>(
                SqlQueries.GetProductDetail,
                new { ProductId = request.ProductId });

            if (data != null)
            {
                return new Response<ProductDtoResponse>(data)
                { State = "Ok", Message = message, Succeeded = true };
            }

            return new Response<ProductDtoResponse>(null)
            { State = "NotFound", Message = "Producto no encontrado", Succeeded = true };
        }
        catch (Exception ex)
        {
            return new Response<ProductDtoResponse>(null)
            { State = "Error", Message = ex.Message, Succeeded = false };
        }
    }

    public async Task<Response<bool>> InsertProduct(ProductAddDtoRequest productAddDtoRequest)
    {
        try
        {
            var product = _mapper.Map<Product>(productAddDtoRequest);
            product.CreatedAt = DateTime.UtcNow;
            product.IsActive = true;

            await _unitOfWork.Repository<Product>().AddAsync(product);


            return new Response<bool>(true) { State = "Ok", Message = "Producto creado", Succeeded = true };
        }
        catch (Exception ex)
        {

            return new Response<bool>(false) { State = "Error", Message = ex.Message, Succeeded = false };
        }
    }

    public async Task<Response<bool>> UpdateProduct(ProductUpdateDtoRequest request)
    {
        try
        {
      
            var (data, message) = await _unitOfWork.Queries.QueryFirstOrDefaultAsync<ProductDtoResponse>(
               SqlQueries.GetProduct,
               new { ProductId = request.ProductId });

            if (data == null)
            {
                return new Response<bool>(false) { State = "Error", Message = "El producto no existe", Succeeded = false };
            }

            var product = _mapper.Map<Product>(request);

            product.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Product>().UpdateAsync(product);


            return new Response<bool>(true) { Message = "Producto actualizado correctamente" };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return new Response<bool>(false) { Message = ex.Message, Succeeded = false };
        }
    }

    public async Task<Response<bool>> BulkInsertProducts(ProductBulkDtoRequest request)
    {
        try
        {
            var faker = new Faker<Product>()
                .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
                .RuleFor(p => p.CategoryId, f => f.Random.Int(1, 5))
                .RuleFor(p => p.SupplierId, f => f.Random.Int(1, 5))
                .RuleFor(p => p.UnitPrice, f => f.Finance.Amount(10, 500))
                .RuleFor(p => p.UnitsInStock, f => (short)f.Random.Number(1, 100))
                .RuleFor(p => p.IsActive, _ => true)
                .RuleFor(p => p.CreatedBy, _ => request.CreatedBy);


            var products = faker.Generate(request.Count);


            await _unitOfWork.Repository<Product>().BulkInsertAsync(products);

            return new Response<bool>(true)
            {
                State = "Ok",
                Message = $"{request.Count} productos insertados con éxito mediante Binary Copy",
                Succeeded = true
            };
        }
        catch (Exception ex)
        {
            return new Response<bool>(false)
            {
                State = "Error",
                Message = $"Error masivo: {ex.Message}",
                Succeeded = false
            };
        }
    }

    public async Task<Response<bool>> UpdateProduct(ProductDeleteDtoRequest request)
    {
        try
        {

            var (data, message) = await _unitOfWork.Queries.QueryFirstOrDefaultAsync<Product>(
                SqlQueries.GetProductById,
                new { ProductId = request.productId });

            if (data == null)
            {
                return new Response<bool>(false)
                {
                    State = "NotFound",
                    Message = "El producto no existe o ya ha sido desactivado",
                    Succeeded = false
                };
            }

            var product = _mapper.Map<Product>(data);

            product.UpdatedAt = DateTime.UtcNow;
            product.IsActive = false;
            product.UpdatedBy = request.updatedBy;

            await _unitOfWork.Repository<Product>().UpdateAsync(product);

            return new Response<bool>(true)
            {
                State = "Ok",
                Message = "Producto desactivado exitosamente",
                Succeeded = true
            };
        }
        catch (Exception ex)
        {

            return new Response<bool>(false)
            {
                State = "Error",
                Message = $"Error al desactivar el producto: {ex.Message}",
                Succeeded = false
            };
        }
    }
}