using CoreLibrary.DTOs.Asisya.Requests;
using CoreLibrary.DTOs.Asisya.Response;
using CoreLibrary.DTOs.Product.Requests;
using Domain.Wrappers;

namespace CoreLibrary.Interface.Services;

public interface IProductService
{
    Task<Response<List<ProductDtoResponse>>> GetProducts(ProductDtoRequest productDtoRequest);

    Task<Response<ProductDtoResponse>> GetProductById(ProductIdDtoRequest productIdDto);

    Task<Response<bool>> InsertProduct(ProductAddDtoRequest productAddDtoRequest);

    Task<Response<bool>> UpdateProduct(ProductUpdateDtoRequest request);

    Task<Response<bool>> BulkInsertProducts(ProductBulkDtoRequest request);

    Task<Response<bool>> UpdateProduct(ProductDeleteDtoRequest request);
}